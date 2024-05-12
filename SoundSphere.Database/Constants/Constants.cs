namespace SoundSphere.Database
{
    public class Constants
    {
        private Constants() { }

        public static readonly string AlbumNotFound = "Album with id {0} not found";
        public static readonly string ArtistNotFound = "Artist with id {0} not found";
        public static readonly string AuthorityNotFound = "Authority with id {0} not found";
        public static readonly string FeedbackNotFound = "Feedback with id {0} not found";
        public static readonly string NotificationNotFound = "Notification with id {0} not found";
        public static readonly string PlaylistNotFound = "Playlist with id {0} not found";
        public static readonly string RoleNotFound = "Role with id {0} not found";
        public static readonly string SongNotFound = "Song with id {0} not found";
        public static readonly string UserNotFound = "User with id {0} not found";

        public static readonly Guid ValidAlbumGuid = Guid.Parse("6ee76a77-2be4-42e3-8417-e60d282cffcb");
        public static readonly Guid ValidArtistGuid = Guid.Parse("4e75ecdd-aafe-4c35-836b-1b83fc7b8f88");
        public static readonly Guid ValidAuthorityGuid = Guid.Parse("75e924c3-34e7-46ef-b521-7331e36caadd");
        public static readonly Guid ValidFeedbackGuid = Guid.Parse("83061e8c-3403-441a-8be5-867ed1f4a86b");
        public static readonly Guid ValidNotificationGuid = Guid.Parse("7e221fa3-2c22-4573-bf21-cd1d6696b576");
        public static readonly Guid ValidPlaylistGuid = Guid.Parse("239d050b-b59c-47e0-9e1a-ab5faf6f903e");
        public static readonly Guid ValidRoleGuid = Guid.Parse("deaf35ba-fe71-4c21-8a3c-d8e5b32a06fe");
        public static readonly Guid ValidSongGuid = Guid.Parse("64f534f8-f2d4-4402-95a3-54de48b678a8");
        public static readonly Guid ValidUserGuid = Guid.Parse("0a9e546f-38b4-4dbf-a482-24a82169890e");
        public static readonly Guid InvalidGuid = Guid.Empty;

        public static readonly string ApiAlbum = "/api/Album";
        public static readonly string ApiArtist = "/api/Artist";
        public static readonly string ApiAuthority = "/api/Authority";
        public static readonly string ApiFeedback = "/api/Feedback";
        public static readonly string ApiNotification = "/api/Notification";
        public static readonly string ApiPlaylist = "/api/Playlist";
        public static readonly string ApiRole = "/api/Role";
        public static readonly string ApiSong = "/api/Song";
        public static readonly string ApiUser = "/api/User";
    }
}