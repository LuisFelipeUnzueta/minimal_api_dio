namespace Minimal.Api.Domain.ModelViews
{
    public record AdminResponse
    {
        public int Id { get; set; } 
        public string Email { get; set; } = string.Empty;
        public string Rule { get; set; } = string.Empty;
    }
}
