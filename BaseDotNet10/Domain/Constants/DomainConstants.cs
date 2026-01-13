namespace Domain.Constants;

/// <summary>
/// ドメイン定数
/// </summary>
public static class DomainConstants
{
    // 結果メッセージ
    public static class Messages
    {
        public const string RegistrationSuccess = "登録が完了しました。確認メールをご確認ください。";
        public const string LoginSuccess = "ログインしました。";
        public const string LogoutSuccess = "ログアウトしました。";
        public const string PasswordResetEmailSent = "パスワードリセット用のメールを送信しました。";
        public const string PasswordResetSuccess = "パスワードをリセットしました。";
        public const string PasswordChangeSuccess = "パスワードを変更しました。";
        public const string EmailConfirmationSuccess = "メールアドレスを確認しました。";
        public const string TwoFactorCodeSent = "認証コードを送信しました。";
    }

    // エラーメッセージ
    public static class Errors
    {
        public const string InvalidCredentials = "メールアドレスまたはパスワードが正しくありません。";
        public const string AccountLocked = "アカウントがロックされています。しばらくしてから再試行してください。";
        public const string EmailNotConfirmed = "メールアドレスが確認されていません。";
        public const string UserNotFound = "ユーザーが見つかりません。";
        public const string InvalidToken = "無効なトークンです。";
        public const string InvalidCode = "無効な認証コードです。";
        public const string CodeExpired = "認証コードの有効期限が切れています。";
    }
}
