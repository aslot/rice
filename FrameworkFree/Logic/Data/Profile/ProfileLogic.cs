using System.IO;
using System;
using SkiaSharp;
using MarkupHandlers;
using Models;
namespace Data
{
    internal sealed class ProfileLogic : IProfileLogic
    {
        private readonly IStorage Storage;
        private readonly ProfileMarkupHandler ProfileMarkupHandler;
        public ProfileLogic(IStorage storage,
                            ProfileMarkupHandler profileMarkupHandler)
        {
            Storage = storage;
            ProfileMarkupHandler = profileMarkupHandler;
        }
        public void Start(in int accountId, in string aboutMe, in bool[] flags, in byte[] file)
        {
            if (Storage.Fast.GetProfilesOnPreSaveLineCount() < Constants.MaxFirstLineLength)
            {
                Storage.Fast.PreSaveProfilesLineEnqueueLocked(
                    new PreProfile
                    {
                        AccountId = accountId,
                        AboutMe = aboutMe,
                        Flags = flags,
                        File = file
                    }
                );
            }
        }
        public void HandleAndSaveProfilesByTimer()
        {
            if (Storage.Fast.GetProfilesOnPreSaveLineCount() > Constants.Zero)
            {
                PreProfile bag = Storage.Fast.PreSaveProfilesLineDequeueLocked();

                if (!string.IsNullOrWhiteSpace(bag.AboutMe)
                    && bag.AboutMe.Length > Constants.Zero
                    && bag.AboutMe.Length <= Constants.MaxReplyMessageTextLength
                    && bag.Flags.Length == Constants.ProfileQuestionsCount
                    && bag.File.Length < Constants.MaxProfileImageSizeBytes
                    && bag.File.Length > Constants.MinProfileImageSizeBytes
                    )
                {
                    Profile profile = new Profile
                    {
                        AccountId = bag.AccountId,
                        PhotoBase64Gif = Convert.ToBase64String(ScaleImageTest(bag.File)),
                        AboutMe = bag.AboutMe,
                        PublicationDate = DateTime.Now,
                        CanPlayChess = bag.Flags[Constants.Zero],
                        WantToMeetInFirstWeek = bag.Flags[Constants.One],
                        CanSupportALargeFamily = bag.Flags[2],
                        HaveBadHabits = bag.Flags[3],
                        HaveChildren = bag.Flags[4],
                        IsAdult = bag.Flags[5],
                        HadRelationship = bag.Flags[6],
                        LikeReading = bag.Flags[7],
                        HaveProfession = bag.Flags[8],
                        HavePermanentResidenceInRussia = bag.Flags[9],
                        DoPhysicalEducation = bag.Flags[10],
                        HaveManyHobbies = bag.Flags[11],
                        SpeakAForeignLanguage = bag.Flags[12],
                        LikeDriving = bag.Flags[13],
                        LikeTravelling = bag.Flags[14],
                        PreferMindActivity = bag.Flags[15],
                        CanMakeMinorRepairs = bag.Flags[16],
                        IsOppositeGenderCute = bag.Flags[17],
                        FollowADiet = bag.Flags[18],
                        HavePets = bag.Flags[19],
                        TakeCareOfPlants = bag.Flags[20],
                        ReadyToWaitForALove = bag.Flags[21],
                        ReadyToSaveFamilyForKids = bag.Flags[22],
                        IsAltruist = bag.Flags[23],
                        AcceptAgression = bag.Flags[24]
                    };
                    Storage.Slow.PutProfileInBase(profile);
                    Storage.Fast.AddOrUpdateOwnProfilePage(profile.AccountId,
                        ProfileMarkupHandler
                            .GetOwnProfileMarkupFromFilledEntity(profile));
                    Storage.Fast.AddOrUpdatePublicProfilePage(profile.AccountId,
                        ProfileMarkupHandler
                            .GetPublicProfileMarkupFromFilledEntity(profile));
                }
            }
        }
        public void LoadProfiles()
        {
            foreach (int accountId in Storage.Fast.IterateThroughAccountIds())
            {
                Profile profile = Storage.Slow.GetProfileOrNullByAccountId(accountId);

                if (profile != null)
                {
                    Storage.Fast.AddOrUpdateOwnProfilePage(accountId,
                    ProfileMarkupHandler.GetOwnProfileMarkupFromFilledEntity(profile));
                    Storage.Fast.AddOrUpdatePublicProfilePage(accountId, ProfileMarkupHandler
                    .GetPublicProfileMarkupFromFilledEntity(profile));
                }
                else
                    Storage.Fast.AddOrUpdateOwnProfilePage(accountId,
                                ProfileMarkupHandler
                                    .GetOwnProfilePrimaryUnfilledMarkup(accountId));
            }
        }

        private byte[] ScaleImageTest(in byte[] file)
        {
            SKBitmap image;

            using (var ms = new MemoryStream(file))
                image = SKBitmap.Decode(ms);
            image = image.Resize(new SKSizeI(
                Constants.ProfileImageWidthPixels,
            Constants.ProfileImageHeightPixels), SKFilterQuality.High);

            using (var ms = new MemoryStream())
            {
                image.Encode(SKEncodedImageFormat.Jpeg, 100).SaveTo(ms);
                image.Dispose();

                return ms.ToArray();
            }
        }
    }
}