# BookManagement
Bài 5&6
<img width="1903" height="1104" alt="image" src="https://github.com/user-attachments/assets/aa33ab9d-9608-42b6-a427-7269324d66bd" />
<img width="1900" height="1111" alt="image" src="https://github.com/user-attachments/assets/45f2aa35-a330-41ec-9f1f-a9ecfea46bac" />
<img width="1905" height="1108" alt="image" src="https://github.com/user-attachments/assets/1be499bc-9e0f-42b1-b711-f6b44892c30e" />
<img width="1898" height="1109" alt="image" src="https://github.com/user-attachments/assets/c0480e59-8f6d-471a-8fad-90c715a8cc4e" />
<img width="1912" height="1113" alt="image" src="https://github.com/user-attachments/assets/ebe9201b-c3d8-4168-a969-aeccbbecfeb7" />
<img width="1907" height="1113" alt="image" src="https://github.com/user-attachments/assets/48f47a5c-723a-4f40-9bc8-3db80b14d099" />
<img width="1903" height="1099" alt="image" src="https://github.com/user-attachments/assets/cdfbc54d-730a-4d96-bfa0-9294f807c909" />
<img width="1903" height="1106" alt="image" src="https://github.com/user-attachments/assets/941b5b81-bfc7-40f4-9540-520059467f53" />
# Giải thích luồng hoạt động code BookManagement

Dự án BookManagement là ứng dụng ASP.NET Core MVC dùng để quản lý sách cơ bản. Dự án có các thư mục chính: Models, Controllers và Views. Repository hiện đang public trên GitHub. 

## 1. Luồng chạy chính

Khi chạy chương trình, file `Program.cs` sẽ khởi tạo ứng dụng ASP.NET Core MVC, đăng ký dịch vụ Controller với View, sau đó cấu hình route mặc định:

/{controller=Home}/{action=Index}/{id?}

Điều này có nghĩa là khi người dùng truy cập website, hệ thống sẽ gọi Controller và Action tương ứng để xử lý yêu cầu.

## 2. Model Book

File `Book.cs` định nghĩa đối tượng sách gồm các thuộc tính:

- Id: mã sách
- Name: tên sách
- Price: giá sách
- Author: tác giả
- Description: mô tả

Trong model có sử dụng validation:

- `Name` bắt buộc nhập, nếu bỏ trống sẽ báo lỗi “Tên không được để trống”.
- `Price` phải lớn hơn 0, nếu nhập sai sẽ báo lỗi “Giá phải lớn hơn 0”.

## 3. BookController

`BookController` là nơi xử lý các chức năng liên quan đến sách.

Ban đầu controller tạo sẵn danh sách sách bằng `List<Book>` gồm 3 sách mẫu: Clean Code, ASP.NET MVC và Design Pattern.

### Chức năng hiển thị danh sách

Khi người dùng vào đường dẫn `/Book/Index`, hàm `Index()` được gọi. Hàm này trả danh sách sách sang View `Index.cshtml` để hiển thị trên giao diện.

### Chức năng xem chi tiết

Khi người dùng bấm “Chi tiết”, hệ thống gọi hàm `Detail(int id)`. Controller sẽ tìm sách theo `Id`. Nếu tìm thấy thì trả dữ liệu sách sang View `Detail.cshtml`; nếu không tìm thấy thì trả về `NotFound()`.

### Chức năng thêm sách

Khi người dùng bấm “Thêm sách mới”, hệ thống gọi hàm `Create()` dạng GET để hiển thị form nhập thông tin sách.

Sau khi người dùng nhập thông tin và bấm lưu, hàm `Create(Book book)` dạng POST được gọi. Hệ thống kiểm tra dữ liệu bằng `ModelState.IsValid`.

- Nếu dữ liệu không hợp lệ, hệ thống trả lại form và hiển thị lỗi validation.
- Nếu dữ liệu hợp lệ, hệ thống tự tạo Id mới, thêm sách vào danh sách, lưu thông báo “Thêm sách thành công!” vào `TempData`, rồi chuyển hướng về trang danh sách.

## 4. View

Thư mục `Views/Book` chứa các giao diện:

- `Index.cshtml`: hiển thị danh sách sách.
- `Create.cshtml`: form thêm sách mới.
- `Detail.cshtml`: hiển thị thông tin chi tiết của một sách.

File `_Layout.cshtml` là giao diện chung của website, có menu Home, Book và Privacy. Khi người dùng chọn Book, hệ thống chuyển đến chức năng quản lý sách.

## 5. Tổng kết luồng hoạt động

Người dùng mở website → chọn Book → hệ thống gọi `BookController.Index()` → hiển thị danh sách sách.

Nếu người dùng chọn “Chi tiết” → hệ thống gửi `id` sách → `BookController.Detail(id)` tìm sách → hiển thị thông tin chi tiết.

Nếu người dùng chọn “Thêm sách mới” → hiển thị form nhập → người dùng nhập tên và giá → controller kiểm tra validation → nếu hợp lệ thì thêm sách vào danh sách và quay lại trang danh sách.

# Middleware trong ASP.NET Core MVC

## Giới thiệu

Trong bài thực hành này, project **BookManagement** được mở rộng bằng cách sử dụng **Middleware** trong ASP.NET Core MVC để:

* Ghi log request.
* Ghi status code sau khi xử lý request.
* Chặn truy cập URL không hợp lệ.
* Hiểu cách hoạt động của middleware pipeline.

---

# Chức năng đã thực hiện

## 1. Ghi log request

Middleware sẽ ghi ra Console thông tin request mỗi khi người dùng truy cập website.

Ví dụ:

```text
[2026-06-05 10:30:15] Method: GET - Path: /Book
[2026-06-05 10:31:02] Method: GET - Path: /Book/Detail/1
```

---

## 2. Ghi status code

Sau khi request được xử lý, middleware tiếp tục ghi status code trả về.

Ví dụ:

```text
Status Code: 200
Status Code: 400
```

---

## 3. Chặn URL không hợp lệ

Nếu người dùng truy cập:

```text
/Book/Detail/0
```

hoặc:

```text
/Book/Detail/-1
```

middleware sẽ:

* Không cho request đi vào Controller.
* Trả về:

```text
Book id khong hop le
```

* Status code:

```text
400
```

---

# Cách hoạt động của Middleware

Middleware hoạt động theo cơ chế pipeline.

Khi request gửi đến:

1. Middleware nhận request.
2. Middleware kiểm tra URL.
3. Nếu URL hợp lệ → chuyển tiếp vào Controller bằng:

```csharp
await _next(context);
```

4. Sau khi Controller xử lý xong → middleware ghi status code.
5. Nếu URL không hợp lệ → middleware trả về response và dùng `return;` để dừng request.

---

# Giải thích các câu hỏi

## Middleware trong ASP.NET Core dùng để làm gì?

Middleware dùng để xử lý request trước khi request đi vào Controller và xử lý response trước khi trả về cho người dùng. Ví dụ: ghi log, kiểm tra quyền truy cập, xử lý lỗi, redirect HTTPS, static files.

---

## Middleware khác Controller ở điểm nào?

Middleware xử lý request ở mức tổng quát trong pipeline, có thể chạy trước nhiều Controller. Controller chỉ xử lý logic cụ thể của từng chức năng, ví dụ xem sách, thêm sách, sửa sách.

---

## Dòng lệnh `await _next(context);` có ý nghĩa gì?

Dòng lệnh này cho phép request tiếp tục đi đến middleware tiếp theo hoặc đi vào Controller. Sau khi Controller xử lý xong, chương trình quay lại middleware để có thể ghi thêm thông tin như status code.

---

## Vì sao khi middleware trả về `return;` thì request không đi tiếp vào Controller?

Vì `return;` kết thúc hàm `InvokeAsync`, nên middleware dừng xử lý tại đó. Do không gọi `await _next(context);`, request sẽ không được chuyển tiếp vào Controller.

---

## Nếu đặt middleware sau `app.MapControllerRoute(...)` thì có thể xảy ra vấn đề gì?

Middleware có thể không chạy đúng hoặc không chặn được request trước khi vào Controller. Vì vậy middleware cần đặt trước `app.MapControllerRoute(...)` để xử lý request trước.

---

## Nếu cần sử dụng thêm middleware khác thì viết tiếp thế nào?

Có thể đăng ký thêm middleware trong `Program.cs`:

```csharp
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<AnotherMiddleware>();
```

Các middleware sẽ chạy theo đúng thứ tự được khai báo.

---






