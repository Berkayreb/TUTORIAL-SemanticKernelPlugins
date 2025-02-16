namespace SemanticKernelPlugins.Server.Data
{
    public class RequestMessage
    {
        public string? MessageRequest { get; set; }
    }

    public class ResponseMessage
    {
        public required string MessageResponse { get; set; }

    }
}
