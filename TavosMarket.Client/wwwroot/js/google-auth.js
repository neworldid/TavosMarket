window.initializeGoogleSignIn = (dotNetHelper, clientId) => {
    if (!window.google) {
        console.error("Google SDK not loaded");
        return;
    }
    google.accounts.id.initialize({
        client_id: clientId,
        callback: (response) => {
            dotNetHelper.invokeMethodAsync('HandleGoogleSignIn', response.credential);
        }
    });
    google.accounts.id.renderButton(
        document.getElementById("googleBtn"),
        { theme: "outline", size: "large", width: "100%", text: "signin_with" }
    );
};
