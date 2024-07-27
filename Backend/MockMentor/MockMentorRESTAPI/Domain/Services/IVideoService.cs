using MockMentorRESTAPI.Domain.DTOs;
using MockMentorRESTAPI.Domain.Models;
using MockMentorRESTAPI.Utilities;

namespace MockMentorRESTAPI.Domain.Services
{
    public interface IVideoService
    {
        Task<Response> AddVideoAsync(AddVideoRequest addVideoRequestvideo);
    }
}
