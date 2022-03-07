using System.ComponentModel.DataAnnotations;
using System;
namespace Own.Database
{
    public partial class Profile
    {
        [Key]
        public int Id { get; set; }
        public int AccountId { get; set; }
        public DateTime PublicationDate { get; set; }
        public string PhotoBase64Jpeg { get; set; }
        public string AboutMe { get; set; }
        public bool CanPlayChess { get; set; }
        public bool WantToMeetInFirstWeek { get; set; }
        public bool CanSupportALargeFamily { get; set; }
        public bool HaveBadHabits { get; set; }
        public bool HaveChildren { get; set; }
        public bool IsAdult { get; set; }
        public bool HadRelationship { get; set; }
        public bool LikeReading { get; set; }
        public bool HaveProfession { get; set; }
        public bool HavePermanentResidenceInRussia { get; set; }
        public bool DoPhysicalEducation { get; set; }
        public bool HaveManyHobbies { get; set; }
        public bool SpeakAForeignLanguage { get; set; }
        public bool LikeDriving { get; set; }
        public bool LikeTravelling { get; set; }
        public bool PreferMindActivity { get; set; }
        public bool CanMakeMinorRepairs { get; set; }
        public bool IsOppositeGenderCute { get; set; }
        public bool FollowADiet { get; set; }
        public bool HavePets { get; set; }
        public bool TakeCareOfPlants { get; set; }
        public bool ReadyToWaitForALove { get; set; }
        public bool ReadyToSaveFamilyForKids { get; set; }
        public bool IsAltruist { get; set; }
        public bool AcceptAgression { get; set; }
    }
}