using MockMentorRESTAPI.Domain.Services;
using MockMentorRESTAPI.Utilities.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockMentorConsumer.Domain.Workflows
{
    public interface IVideoConversionWorkflow
    {
        Task HandleVideoConversionResponse(VideoConversionQueueResponse videoConversionQueueResponse, ISQSService sQSService);
    }
}
