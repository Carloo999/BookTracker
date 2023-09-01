using System.ComponentModel.DataAnnotations;

namespace BookTracker.App.Enums;

public enum PrivacyStatus
{
    Private,
    Public,
    [Display(Name = "Friends Only")]
    FriendsOnly
}