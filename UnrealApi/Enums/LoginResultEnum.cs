namespace InternalApi.Enums
{
    public enum LoginResultEnum
    {
        None = 0,
        Ok = 1,
        WrongCredentials = 2,
        UnauthorizedUser = 3,
        AuthenticationLocked = 4,
        UnavailableServer = 5,
        OtherException = 6,
        MessageSecurityException = 7
    }
}
