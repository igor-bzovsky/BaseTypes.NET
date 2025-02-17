namespace BaseTypes.NET.Tests
{
    public class ErrorTests
    {
        [Fact]
        public void Error_should_have_correct_code_and_message()
        {
            var error = new Error("404", "Resource not found");
            error.Code.Should().Be("404");
            error.Message.Should().Be("Resource not found");
        }
    }
}
