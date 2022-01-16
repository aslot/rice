using System;
using System.Text;
using Data;
using MarkupHandlers;
using XXHash;

internal sealed class Captcha
{
    private readonly ICaptchaGenerator CaptchaGenerator;
    private readonly IStorage Storage;
    private readonly CaptchaMarkupHandler CaptchaMarkupHandler;
    public Captcha(ICaptchaGenerator captchaGenerator,
    IStorage storage,
    CaptchaMarkupHandler captchaMarkupHandler)
    {
        CaptchaGenerator = captchaGenerator;
        Storage = storage;
        CaptchaMarkupHandler = captchaMarkupHandler;
    }

    internal CaptchaStringAndImage GenerateCaptchaStringAndImage()
    {
        var captchaString = GenerateCaptchaString();
        var bag = new CaptchaStringAndImage
        {
            stringHash = XXHash32.Hash(captchaString),
            image = CaptchaMarkupHandler.GenerateCaptchaMarkup(
                Convert.ToBase64String(
                CaptchaGenerator.GenerateImageAsByteArray(captchaString)))
        };

        return bag;
    }


    public string GenerateCaptchaString()
    {
        StringBuilder sb = new StringBuilder();

        for (byte i = Constants.Zero; i < Constants.CaptchaLength; i++)
            sb.Append(Storage.Fast.GetNextRandomCaptchaSymbol());

        return sb.ToString();
    }
}