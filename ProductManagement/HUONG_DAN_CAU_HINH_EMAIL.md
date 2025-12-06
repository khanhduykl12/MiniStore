# Hướng dẫn cấu hình Gmail SMTP cho chức năng Quên mật khẩu

## Vấn đề: Lỗi "Failure sending mail" hoặc "5.7.0 Authentication Required"

## Giải pháp:

### Bước 1: Bật 2-Step Verification cho tài khoản Gmail

1. Đăng nhập vào tài khoản Gmail: https://myaccount.google.com
2. Vào **Security** (Bảo mật)
3. Tìm mục **2-Step Verification** (Xác minh 2 bước)
4. Bật tính năng này nếu chưa bật
5. Làm theo hướng dẫn để thiết lập

### Bước 2: Tạo App Password

1. Sau khi bật 2-Step Verification, quay lại trang **Security**
2. Tìm mục **App passwords** (Mật khẩu ứng dụng)
   - Nếu không thấy, tìm kiếm "App passwords" trong trang Security
3. Chọn **Mail** làm ứng dụng
4. Chọn **Other (Custom name)** và nhập tên: "MiniStore"
5. Nhấn **Generate** (Tạo)
6. Google sẽ hiển thị một mật khẩu 16 ký tự (không có dấu cách)
   - Ví dụ: `abcd efgh ijkl mnop` → sử dụng: `abcdefghijklmnop`

### Bước 3: Cập nhật App Password trong code

1. Mở file: `ProductManagement/Forms/Login + Register/PasswordForgot.cs`
2. Tìm dòng 114: `string appPassword = "puhzemgfwkymgmkj";`
3. Thay thế bằng App Password mới bạn vừa tạo (16 ký tự, không có dấu cách)
4. Lưu file và chạy lại ứng dụng

### Lưu ý quan trọng:

- ✅ App Password phải là 16 ký tự liền nhau, KHÔNG có dấu cách
- ✅ App Password chỉ hiển thị 1 lần khi tạo, hãy copy ngay
- ✅ Nếu quên App Password, phải tạo lại App Password mới
- ✅ Mỗi App Password chỉ dùng cho 1 ứng dụng
- ✅ App Password không phải là mật khẩu Gmail thông thường

### Kiểm tra:

Sau khi cập nhật, thử lại chức năng "Quên mật khẩu":

1. Nhập email hợp lệ
2. Nhấn "Gửi"
3. Kiểm tra email để nhận mã OTP

### Nếu vẫn còn lỗi:

1. Kiểm tra kết nối internet
2. Kiểm tra tường lửa/antivirus có chặn kết nối SMTP không
3. Tạo App Password mới và cập nhật lại
4. Kiểm tra email gửi đi (`lamthuan271019@gmail.com`) có hoạt động bình thường không
