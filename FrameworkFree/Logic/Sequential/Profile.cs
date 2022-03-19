using Own.Storage;
using Own.Types;
using Own.Permanent;
using System;
using Inclusions;
using Own.MarkupHandlers;
namespace Own.Sequential
{
    internal static partial class Unstable
    {
        internal static void HandleAndSaveProfilesByTimerVoid()
        {
            if (Fast.GetProfilesOnPreSaveLineCountLocked() > Constants.Zero)
            {
                PreProfile bag = Fast.PreSaveProfilesLineDequeueLocked();

                if (!string.IsNullOrWhiteSpace(bag.AboutMe)
                    && bag.AboutMe.Length > Constants.Zero
                    && bag.AboutMe.Length <= Constants.MaxReplyMessageTextLength
                    && bag.Flags.Length == Constants.ProfileQuestionsCount
                    && bag.File.Length < Constants.MaxProfileImageSizeBytes
                    && bag.File.Length > Constants.MinProfileImageSizeBytes
                    )
                {
                    Own.Database.Profile profile = new Own.Database.Profile
                    {
                        AccountId = bag.AccountId,
                        PhotoBase64Jpeg = Convert.ToBase64String(Graphics.GetScaledProfileImage(bag.File)),
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
                    Slow.PutProfileInBaseVoid(profile);
                    Fast.AddOrUpdateOwnProfilePageLocked(profile.AccountId,
                        Marker
                            .GetOwnProfileMarkupFromFilledEntity(profile));
                    Fast.AddOrUpdatePublicProfilePageLocked(profile.AccountId,
                        Marker
                            .GetPublicProfileMarkupFromFilledEntity(profile));
                }
            }
        }

        internal static void LoadProfilesVoid()
        {
            foreach (int accountId in Fast.IterateThroughAccountIdsLocked())
            {
                Own.Database.Profile profile = Slow.GetProfileOrNullByAccountIdNullable(accountId);

                if (profile != null)
                {
                    Fast.AddOrUpdateOwnProfilePageLocked(accountId,
                    Marker.GetOwnProfileMarkupFromFilledEntity(profile));
                    Fast.AddOrUpdatePublicProfilePageLocked(accountId, Marker
                    .GetPublicProfileMarkupFromFilledEntity(profile));
                }
                else
                    Fast.AddOrUpdateOwnProfilePageLocked(accountId,
                                Marker
                                    .GetOwnProfilePrimaryUnfilledMarkup(accountId));
            }
        }
    }
}