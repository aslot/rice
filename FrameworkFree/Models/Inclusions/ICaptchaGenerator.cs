public interface ICaptchaGenerator
{
    byte[] GenerateImageAsByteArray(string captchaCode, int width = 150, int height = 50);
}