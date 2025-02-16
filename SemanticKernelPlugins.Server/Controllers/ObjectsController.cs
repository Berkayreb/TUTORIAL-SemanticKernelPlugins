#pragma warning disable SKEXP0070

using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.Ollama;
using SemanticKernelPlugins.Server.Data;

namespace SemanticKernelPlugins.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ObjectsController : ControllerBase
    {


        private readonly Kernel _kernel;
        public ObjectsController(Kernel kernel)
        {
            _kernel = kernel;
        }

        [HttpPost("requestforhome")]
        public async Task<IActionResult> Requestforhome([FromBody] RequestMessage request)
        {

            if (string.IsNullOrWhiteSpace(request.MessageRequest))
                return BadRequest("request cannot be empty.");

            var chatCompletionService = _kernel.GetRequiredService<IChatCompletionService>();
            var settings = new OllamaPromptExecutionSettings { FunctionChoiceBehavior = FunctionChoiceBehavior.Auto() };

            var history = new ChatHistory();
            history.AddUserMessage(request.MessageRequest);

            ChatMessageContent chatResult = await chatCompletionService.GetChatMessageContentAsync(history, settings, _kernel);
            history.AddMessage(chatResult.Role, chatResult.Content);
            return Ok(new ResponseMessage { MessageResponse= chatResult.Content});
        }
    }
}
