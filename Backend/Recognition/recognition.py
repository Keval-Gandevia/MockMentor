import boto3
import cv2
import os
import urllib.parse
import json
from collections import defaultdict
from botocore.exceptions import NoCredentialsError, PartialCredentialsError, ClientError

region_name = os.getenv('AWS_DEFAULT_REGION')
aws_access_key_id = os.getenv('AWS_ACCESS_KEY_ID')
aws_secret_access_key = os.getenv('AWS_SECRET_ACCESS_KEY')
aws_session_token = os.getenv('AWS_SESSION_TOKEN')

rekognition = boto3.client(
    'rekognition',
    aws_access_key_id=aws_access_key_id,
    aws_secret_access_key=aws_secret_access_key,
    aws_session_token=aws_session_token,
    region_name=region_name
)

s3 = boto3.client(
    's3',
    aws_access_key_id=aws_access_key_id,
    aws_secret_access_key=aws_secret_access_key,
    aws_session_token=aws_session_token,
    region_name=region_name
)

sqs = boto3.client(
    'sqs',
    aws_access_key_id=aws_access_key_id,
    aws_secret_access_key=aws_secret_access_key,
    aws_session_token=aws_session_token,
    region_name=region_name
)

def download_video_from_s3(video_url, download_path):
    print(f"Downloading video from S3: {video_url}")
    try:
        parsed_url = urllib.parse.urlparse(video_url)
        bucket_name = parsed_url.netloc.split('.')[0]
        video_key = parsed_url.path.lstrip('/')
        s3.download_file(bucket_name, video_key, download_path)
        print(f"Video downloaded successfully to {download_path}")
    except NoCredentialsError:
        print("Credentials not available")
    except PartialCredentialsError:
        print("Incomplete credentials provided")
    except ClientError as e:
        print(f"ClientError: {e}")
    except Exception as e:
        print(f"Error downloading video: {str(e)}")

def get_frames(video_path):
    print(f"Getting frames from video: {video_path}")
    video_capture = cv2.VideoCapture(video_path)
    frames = []
    success, frame = video_capture.read()
    while success:
        frames.append(frame)
        success, frame = video_capture.read()
    video_capture.release()
    print(f"Extracted {len(frames)} frames from video")
    return frames

def detect_emotions_in_frame(frame):
    print("Detecting emotions in frame")
    _, encoded_image = cv2.imencode('.jpg', frame)
    response = rekognition.detect_faces(
        Image={'Bytes': encoded_image.tobytes()},
        Attributes=['ALL']
    )
    emotions = []
    if 'FaceDetails' in response:
        for face_detail in response['FaceDetails']:
            emotions.extend(face_detail['Emotions'])
    print(f"Detected {len(emotions)} emotions in frame")
    return emotions

def analyze_video_emotions(video_url):
    print(f"Analyzing emotions in video: {video_url}")
    download_path = 'video.mp4'
    download_video_from_s3(video_url, download_path)
    
    frames = get_frames(download_path)
    all_emotions = []
    
    for frame in frames:
        emotions = detect_emotions_in_frame(frame)
        all_emotions.extend(emotions)

    if os.path.exists(download_path):
        os.remove(download_path)
        print(f"Removed temporary video file: {download_path}")
    
    return all_emotions

def find_average_emotion(emotions):
    print("Finding average emotion")
    emotion_confidences = defaultdict(list)
    for emotion in emotions:
        emotion_confidences[emotion['Type']].append(emotion['Confidence'])
    
    average_confidence = {emotion: sum(confidences) / len(confidences) 
                          for emotion, confidences in emotion_confidences.items()}
    
    average_emotion = max(average_confidence, key=average_confidence.get)
    print(f"Average emotion: {average_emotion}")
    return average_emotion

def create_queue_if_not_exists(queue_name):
    response = sqs.list_queues(QueueNamePrefix=queue_name)
    if 'QueueUrls' in response:
        for url in response['QueueUrls']:
            if queue_name in url:
                return url
    
    response = sqs.create_queue(QueueName=queue_name)
    return response['QueueUrl']

def send_response(response_queue_url, message):
    print(f"Sending response to queue URL: {response_queue_url}")
    sqs.send_message(
        QueueUrl=response_queue_url,
        MessageBody=json.dumps(message)
    )

def process_message(message, response_queue_url):
    body = json.loads(message['Body'])
    question_id = body.get('questionId')
    video_url = body.get('videoUrl')
    messageType = body.get('messageType')

    video_id = video_url.split('/')[-1].split('_')[0]

    emotions = analyze_video_emotions(video_url)
    average_emotion = find_average_emotion(emotions)

    response = {
        'questionId': question_id,
        'videoId': video_id,
        'emotionValue': average_emotion,
        'messageType': messageType
    }
    send_response(response_queue_url, response)

def listen_for_messages(request_queue_name, response_queue_name):
    request_queue_url = create_queue_if_not_exists(request_queue_name)
    response_queue_url = create_queue_if_not_exists(response_queue_name)
    
    while True:
        response = sqs.receive_message(
            QueueUrl=request_queue_url,
            AttributeNames=['All'],
            MaxNumberOfMessages=1,
            WaitTimeSeconds=20
        )

        if 'Messages' in response:
            for message in response['Messages']:
                process_message(message, response_queue_url)
                sqs.delete_message(
                    QueueUrl=request_queue_url,
                    ReceiptHandle=message['ReceiptHandle']
                )

if __name__ == "__main__":
    request_queue_name = 'emotion-request-queue'
    response_queue_name = 'emotion-response-queue'
    listen_for_messages(request_queue_name, response_queue_name)
