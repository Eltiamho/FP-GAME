Imports MySql.Data.MySqlClient

Module ModuleKoneksi
    'Deklarasi variabel koneksi string untuk digunakan dalam seluruh aplikasi
    Private connectionString As String = "server=localhost;user id=root;" & "password=;database=vstudio;"
    ' Fungsi untuk mendapatkan objek MySqlConnection
    Public Function GetConnection() As MySqlConnection
        Dim conn As New MySqlConnection(connectionString)
        Try
            ' Buka koneksi
            conn.Open()
        Catch ex As MySqlException
            'Jika gagal koneksi, tampilkan pesan error
            MessageBox.Show("Error: " & ex.Message)
        End Try
        'Kembalikan objek koneksi yang sudah dibuka
        Return conn
    End Function
End Module
Public Class Form1

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        'Panggil fungsi koneksi dari modul modKoneksi
        Dim conn As MySqlConnection = GetConnection()
        If conn.State = ConnectionState.Open Then
            Console.WriteLine("Koneksi berhasil!")
        Else
            Console.WriteLine("Koneksi gagal.")
        End If
        'Pastikan untuk menutup koneksi setelah selesai digunakan
        conn.Close()
    End Sub
    Private Function AuthenticateUser(username As String, password As String) As Boolean
        Dim query As String = "SELECT COUNT(*) FROM users WHERE username=@username
AND password=@password"
        'Panggil koneksi dari modul modKoneksi
        Dim conn As MySqlConnection = GetConnection()
        Using command As New MySqlCommand(query, conn)
            ' Parameterisasi query untuk mencegah SQL Injection
            command.Parameters.AddWithValue("@username", username)
            command.Parameters.AddWithValue("@password", password)
            Try
                'Eksekusi query dan ambil hasilnya
                Dim result As Integer = Convert.ToInt32(command.ExecuteScalar())
                'Jika hasilnya lebih besar dari 0, user ditemukan
                Return result > 0
            Catch ex As MySqlException
                MessageBox.Show("Error saat login: " & ex.Message)
                Return False
            Finally
                'Pastikan koneksi ditutup
                conn.Close()
            End Try
        End Using
    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim username As String = textUser.Text
        Dim password As String = txtPass.Text
        'Check if fields are not empty
        If String.IsNullOrEmpty(username) Or String.IsNullOrEmpty(password) Then
            MessageBox.Show("Username and password cannot be empty.")
            Exit Sub
        End If
        'Autentifikasi user
        If AuthenticateUser(username, password) Then
            'Jika login berhasil
            MessageBox.Show("Login successful!")
            Me.Hide()
            Form2.Show()
        Else
            'Jika login failed, show error message
            MessageBox.Show("Invalid username or password.")
        End If
    End Sub
End Class
