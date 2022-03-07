using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Net;
using Inclusions;
using Own.Database;
using Own.Types;
using Own.Permanent;
using static Own.DataLockers.Lockers;
namespace Own.Storage
{
    internal static class Slow
    {
        internal static int GetAccountsCount()
        {
            using (var bag = new TotalForumDbContext())
                return bag.Account.AsNoTracking().Count();
        }
        internal static void CheckAccountIdVoid(Func<Pair, int?> getAccIdAndStoreSlow)
        {
            lock (LoginPasswordHashesDeltaLocker)
            {
                IEnumerable<Pair> keys = Fast.GetLoginPasswordHashesDeltaKeysLocked();

                foreach (Pair pair in keys)
                {
                    if (!Fast.LoginPasswordAccIdHashesContainsKeyLocked(pair))
                    {
                        getAccIdAndStoreSlow(pair);
                    }
                }
            }
        }
        internal static void RemoveAccountByNickIfExistsVoid(string nick)
        {
            using (var bag = new TotalForumDbContext())
            {
                if (bag.Account.AsNoTracking().Count(account => account.Nick == nick) == Constants.One)
                {
                    bag.Account.Remove(new Account { Nick = nick });
                    bag.SaveChanges();
                }
            }
        }
        internal static ICollection<Pair> GetPairsNullable()
        {
            using (var bag = new TotalForumDbContext())
                return bag.Account.AsNoTracking().Select(a => new Pair { LoginHash = unchecked((uint)a.Identifier), PasswordHash = unchecked((uint)a.Passphrase) }).ToList();
        }
        internal static void InitializeBlockedIpsHashesVoid()
        {
            using (var bag = new TotalForumDbContext())
                Fast.BlockedRemoteIpsHashesStatic = bag.BlockedIpHash.AsNoTracking().Select(i => unchecked((uint)i.IpHash)).ToList();
        }
        internal static IEnumerable<string> GetNicksNullable()
        {
            using (var bag = new TotalForumDbContext())
                return bag.Account.AsNoTracking().Select(a => a.Nick).ToList();
        }
        internal static int? GetAccountIdFromBaseNullable
                (uint loginHash, uint passwordHash)
        {
            int? result = null;

            using (var bag = new TotalForumDbContext())
            {
                Account temp = null;
                if (bag.Account != null)
                    temp = bag.Account.AsNoTracking().FirstOrDefault(unchecked(a => a.Identifier == unchecked((int)loginHash) && a.Passphrase == unchecked((int)passwordHash)));
                else
                { }

                if (temp != null)
                    result = temp.Id;
            }
            return result;
        }
        internal static IEnumerable<IdName> GetEndpointIdNamesNullable(int id)
        {
            using (var bag = new TotalForumDbContext())
                return bag.Endpoint.AsNoTracking().Where(e => e.ForumId == id).OrderBy(b => b.Id).Select(a =>
                   new IdName { Id = a.Id, Name = a.Name }).Take(Constants.five).ToList();
        }
        internal static IEnumerable<IdName> GetForumIdNamesNullable()
        {
            using (var bag = new TotalForumDbContext())
                return bag.Forum.AsNoTracking()
                    .OrderBy(f => f.Id).Take(Constants.five)
                    .Select(a => new IdName { Id = a.Id, Name = a.Name }).ToList();
        }
        internal static bool CheckNickInBase(string nick)
        {
            using (var bag = new TotalForumDbContext())
                return bag.Account.AsNoTracking().Any(a => a.Nick == nick);
        }
        internal static int GetIdByNick(string nick)
        {
            using (var bag = new TotalForumDbContext())
                return bag.Account.AsNoTracking().First(a => a.Nick == nick).Id;
        }
        internal static void PutPrivateMessageInBaseVoid
            (int senderAccId, int acceptorAccId, string privateText)
        {
            using (var bag = new TotalForumDbContext())
            {
                bag.Add(new PrivateMessage()
                {
                    SenderAccountId = senderAccId,
                    AcceptorAccountId = acceptorAccId,
                    PrivateText = privateText
                });
                bag.SaveChanges();
            }
        }
        internal static void PutAccountIdentifierIpHashInBaseIfNotExistsVoid
            (uint accountIdentifierHash, IPAddress ip)
        {
            using (var bag = new TotalForumDbContext())
            {
                var loginLog = new LoginLog()
                {
                    AccountIdentifier = unchecked((int)accountIdentifierHash),
                    IpHash = unchecked((int)XXHash32.Hash(ip.ToString()))
                };

                if (bag.LoginLog.AsNoTracking().FirstOrDefault
                (p => p.AccountIdentifier == loginLog.AccountIdentifier
                && p.IpHash == loginLog.IpHash) == null)
                {
                    bag.Add(loginLog);
                    bag.SaveChanges();
                }
            }
        }
        internal static int PutThreadAndMessageInBase(Thread thread, int accountId, string message)
        {
            using (var bag = new TotalForumDbContext())
            {
                bag.Thread.Add(thread);
                bag.SaveChanges();
                bag.Msg.Add(new Msg { ThreadId = thread.Id, AccountId = accountId, MsgText = message });
                bag.SaveChanges();
            }

            return thread.Id;
        }
        internal static int CountPrivateMessages(int id)
        {
            using (var bag = new TotalForumDbContext())
                return bag.PrivateMessage.AsNoTracking().Where(pm => pm.AcceptorAccountId == id).Select(i => new { Id = i.SenderAccountId })
                                              .Union(bag.PrivateMessage.AsNoTracking().Where(pm => pm.SenderAccountId == id).Select(i => new { Id = i.AcceptorAccountId }))
                               .Intersect(bag.Account.AsNoTracking().Select(a => new { Id = a.Id }))
                               .Count();
        }
        internal static IEnumerable<IdText> GetPrivateMessagesByIdsNullable(int companionId, int accountId)
        {
            using (var bag = new TotalForumDbContext())
                return bag.PrivateMessage.AsNoTracking()
                    .Where(pm => pm.SenderAccountId == accountId
                        && pm.AcceptorAccountId == companionId
                        || pm.SenderAccountId == companionId
                        && pm.AcceptorAccountId == accountId).OrderBy(i => i.Id)
                    .Select(m => new IdText
                    {
                        SenderAccountId = m.SenderAccountId,
                        PrivateText = m.PrivateText
                    }).ToList();
        }
        internal static void AddAccountVoid(Account dbAccount)
        {
            using (var bag = new TotalForumDbContext())
            {
                bag.Account.Add(dbAccount);
                bag.SaveChanges();
            }
        }
        internal static void PutMessageInBaseVoid(Msg dbMessage)
        {
            using (var bag = new TotalForumDbContext())
            {
                bag.Msg.Add(dbMessage);
                bag.SaveChanges();
            }
        }
        internal static IEnumerable<IdNick> GetIdNicksByAccountIdNullable(int id)
        {
            using (var bag = new TotalForumDbContext())
                return bag.Account.AsNoTracking()
                        .Join(bag.PrivateMessage.AsNoTracking()
                            .Where(m => m.AcceptorAccountId == id),
                                account => account.Id, pm => pm.SenderAccountId,
                                (account, pm) => new IdNick { Id = account.Id, Nick = account.Nick })
                                  .Union(bag.Account.AsNoTracking()
                                  .Join(bag.PrivateMessage.AsNoTracking()
                                  .Where(m => m.SenderAccountId == id), account => account.Id,
                                  pm => pm.AcceptorAccountId, (account, pm) =>
                                    new IdNick { Id = account.Id, Nick = account.Nick })).Distinct().ToList();
        }
        internal static CompanionId[] GetCompanionsByAccountIdNullable(int accountId)
        {
            using (var bag = new TotalForumDbContext())
                return bag.Account.AsNoTracking().Join(bag.PrivateMessage.AsNoTracking().Where(pm => pm.AcceptorAccountId == accountId),
                    account => account.Id, message => message.SenderAccountId, (account, message) => new CompanionId { Id = account.Id })
                    .Union(bag.Account.AsNoTracking().Join(bag.PrivateMessage.AsNoTracking().Where(pm => pm.SenderAccountId == accountId),
                    account => account.Id, message => message.AcceptorAccountId, (account, message) => new CompanionId { Id = account.Id })).Distinct().ToArray();

        }
        internal static int CountPrivateMessagesByIds(int companionId, int accountId)
        {
            using (var bag = new TotalForumDbContext())
                return bag.PrivateMessage.AsNoTracking()
                    .Count(pm => pm.SenderAccountId == accountId
                    && pm.AcceptorAccountId == companionId
                    || pm.SenderAccountId == companionId
                    && pm.AcceptorAccountId == accountId);
        }
        internal static int CountThreadsById(int id)
        {
            using (var bag = new TotalForumDbContext())
                return bag.Thread.AsNoTracking().Count(t => t.EndpointId == id);
        }
        internal static IEnumerable<IdName> GetThreadIdNamesByIdNullable(int id)
        {
            using (var bag = new TotalForumDbContext())
                return bag.Thread.AsNoTracking()
                    .Where(t => t.EndpointId == id)
                    .Select(i => new IdName { Id = i.Id, Name = i.Name })
                    .OrderByDescending(j => j.Id).ToList();
        }
        internal static int CountMessagesByAmount(int amount)
        {
            using (var bag = new TotalForumDbContext())
                return bag.Msg.AsNoTracking().Count(m => m.ThreadId == amount);
        }
        internal static IEnumerable<Message> GetMessagesByAmountNullable(int amount)
        {
            using (var bag = new TotalForumDbContext())
                return bag.Msg.AsNoTracking()
                    .Where(m => m.ThreadId == amount)
                    .Select(msg => new Message
                    {
                        Id = msg.Id,
                        AccountId = msg.AccountId,
                        MsgText = msg.MsgText
                    })
                    .OrderBy(ms => ms.Id).ToList();
        }
        internal static int GetSectionNumById(int id)
        {
            using (var bag = new TotalForumDbContext())
                return bag.Thread.AsNoTracking().First(t => t.Id == id).EndpointId;
        }
        internal static string GetThreadNameByIdNullable(int id)
        {
            using (var bag = new TotalForumDbContext())
                return bag.Thread.AsNoTracking().First(t => t.Id == id).Name;
        }
        internal static string GetNickByAccountIdNullable(int accountId)
        {
            using (var bag = new TotalForumDbContext())
                return bag.Account.AsNoTracking().First(a => a.Id == accountId).Nick;
        }
        internal static int GetThreadsCount()
        {
            using (var bag = new TotalForumDbContext())
                return bag.Thread.AsNoTracking().Count();
        }
        internal static IEnumerable<int> GetExistingThreadsIdsNullable()
        {
            using (var bag = new TotalForumDbContext())
                return bag.Thread.AsNoTracking().Select(t => t.Id).ToList();
        }
        internal static Profile GetProfileOrNullByAccountIdNullable(int id)
        {
            using (var bag = new TotalForumDbContext())
                return bag.Profile.AsNoTracking().FirstOrDefault(p => p.AccountId == id);
        }
        internal static void PutProfileInBaseVoid(Profile profile)
        {
            using (var bag = new TotalForumDbContext())
            {
                Profile retrieved = bag.Profile.FirstOrDefault(p => p.AccountId == profile.AccountId);

                if (retrieved == null)
                    bag.Profile.Add(profile);
                else
                {
                    retrieved.AccountId = profile.AccountId;
                    retrieved.AboutMe = profile.AboutMe;
                    retrieved.AcceptAgression = profile.AcceptAgression;
                    retrieved.CanMakeMinorRepairs = profile.CanMakeMinorRepairs;
                    retrieved.CanPlayChess = profile.CanPlayChess;
                    retrieved.CanSupportALargeFamily = profile.CanSupportALargeFamily;
                    retrieved.DoPhysicalEducation = profile.DoPhysicalEducation;
                    retrieved.FollowADiet = profile.FollowADiet;
                    retrieved.HadRelationship = profile.HadRelationship;
                    retrieved.HaveBadHabits = profile.HaveBadHabits;
                    retrieved.HaveChildren = profile.HaveChildren;
                    retrieved.HaveManyHobbies = profile.HaveManyHobbies;
                    retrieved.HavePermanentResidenceInRussia = profile.HavePermanentResidenceInRussia;
                    retrieved.HavePets = profile.HavePets;
                    retrieved.HaveProfession = profile.HaveProfession;
                    retrieved.IsAdult = profile.IsAdult;
                    retrieved.IsAltruist = profile.IsAltruist;
                    retrieved.IsOppositeGenderCute = profile.IsOppositeGenderCute;
                    retrieved.LikeDriving = profile.LikeDriving;
                    retrieved.LikeReading = profile.LikeReading;
                    retrieved.LikeTravelling = profile.LikeTravelling;
                    retrieved.SpeakAForeignLanguage = profile.SpeakAForeignLanguage;
                    retrieved.TakeCareOfPlants = profile.TakeCareOfPlants;
                    retrieved.WantToMeetInFirstWeek = profile.WantToMeetInFirstWeek;
                    retrieved.ReadyToWaitForALove = profile.ReadyToWaitForALove;
                    retrieved.PublicationDate = DateTime.Now;
                    retrieved.ReadyToSaveFamilyForKids = profile.ReadyToSaveFamilyForKids;
                    retrieved.PhotoBase64Jpeg = profile.PhotoBase64Jpeg;
                    retrieved.PreferMindActivity = profile.PreferMindActivity;
                }

                bag.SaveChanges();
            }
        }
    }
}