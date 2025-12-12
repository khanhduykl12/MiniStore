namespace MiniStore.Class
{
    public static class UserSession
    {
        // current user role: "ADMIN", "NV", "KH"
    public static string Role { get; set; } = "ADMIN";

    // username đăng nhập
    public static string Username { get; set; } = string.Empty;

    // họ tên nhân viên/ người dùng
    public static string FullName { get; set; } = string.Empty;
    }
}