import os
import boto3
import json
import ffmpeg
import uuid
import time

# AWS credentials and region configuration
region_name = os.getenv('AWS_DEFAULT_REGION')
aws_access_key_id = os.getenv('AWS_ACCESS_KEY_ID')
aws_secret_access_key = os.getenv('AWS_SECRET_ACCESS_KEY')
aws_session_token = os.getenv('AWS_SESSION_TOKEN')

s3 = boto3.client('s3', region_name=region_name, aws_access_key_id=aws_access_key_id, aws_secret_access_key=aws_secret_access_key, aws_session_token=aws_session_token)
rekognition = boto3.client('rekognition', region_name=region_name, aws_access_key_id=aws_access_key_id, aws_secret_access_key=aws_secret_access_key, aws_session_token=aws_session_token)

def convert_webm_to_mp4(input_url, output_path):
    try:
        ffmpeg.input(input_url).output(output_path).run(overwrite_output=True)
    except ffmpeg.Error as e:
        print(f"Error converting video: {e}")
        raise

def detect_emotion_from_video(bucket_name, video_url):
    # Convert WebM to MP4
    object_name = video_url.split('/')[-1]
    mp4_object_name = f"{uuid.uuid4()}.mp4"
    mp4_path = f"/tmp/{mp4_object_name}"

    # Download the video from S3
    s3.download_file(bucket_name, object_name, f"/tmp/{object_name}")

    # Convert the downloaded video to MP4
    convert_webm_to_mp4(f"/tmp/{object_name}", mp4_path)

    # Upload the converted video back to S3
    s3.upload_file(mp4_path, bucket_name, mp4_object_name)

    # Start face detection in the video using AWS Rekognition
    response = rekognition.start_face_detection(
        Video={'S3Object': {'Bucket': bucket_name, 'Name': mp4_object_name}}
    )

    job_id = response['JobId']
    
    # Wait for the job to complete
    start_time = time.time()
    while True:
        response = rekognition.get_face_detection(JobId=job_id)
        if response['JobStatus'] in ['SUCCEEDED', 'FAILED']:
            break
        if time.time() - start_time > 600:  # Timeout after 10 minutes
            raise TimeoutError('Face detection job timed out')
        time.sleep(5)
    
    if response['JobStatus'] == 'FAILED':
        print("Job failed with response:", response)
        raise Exception('Face detection job failed')
    
    if not response['Faces']:
        raise Exception('No faces detected in the video')
    
    # Extract dominant emotion from detected faces
    emotions = response['Faces'][0]['Emotions']
    dominant_emotion = max(emotions, key=lambda e: e['Confidence'])['Type']
    
    return dominant_emotion

def process_emotion_message():
    # Hardcoded response body for testing
    body = {
        'videoUrl': 'https://mock-mentor-bucket.s3.amazonaws.com/20_video.webm',
        'videoId': 20,
        'questionId': '20'
    }
    
    video_url = body.get('videoUrl')
    video_id = body.get('videoId')
    bucket_name = 'mock-mentor-bucket'

    dominant_emotion = detect_emotion_from_video(bucket_name, video_url)

    # Print the response
    response = {
        'videoId': video_id,
        'emotion': dominant_emotion
    }
    
    print(json.dumps(response, indent=4))

if __name__ == '__main__':
    process_emotion_message()
