namespace MediaDataApplication.AspNetMvcClient.Global.Authentication {

    public interface IAuthenticationManager {
        bool SignIn(string userName, string password, bool isPersistent);

        void SignOut();
    }

}