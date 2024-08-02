﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MockMentorRESTAPI.Domain.Models;
using MockMentorRESTAPI.Domain.Services;
using MockMentorRESTAPI.Utilities;
using MockMentorRESTAPI.Utilities.RequestModels;
using System.Diagnostics;

namespace MockMentorRESTAPI.Controllers
{
    [Route(APIRoutes.CONTROLLER)]
    [ApiController]
    public class VideosController : ControllerBase
    {
        private readonly IVideoService _videoService;

        public VideosController(IVideoService videoService)
        {
            _videoService = videoService;
        }

        [HttpPost(APIRoutes.ADD_VIDEO)]
        public async Task<Response> AddVideo([FromBody] AddVideoRequest addVideoRequest)
        {
            return await _videoService.AddVideoAsync(addVideoRequest);
        }
    }
}
