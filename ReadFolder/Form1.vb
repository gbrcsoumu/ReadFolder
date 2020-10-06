Imports System
Imports System.IO
Imports System.Net


Public Class Form1
    Private filename() As String, fname() As String, dir1() As String
    Private Path1 As String, Path2 As String
    Private Result As String, nn As Integer
    Private ip As String
    Private Const Nas1 As String = "\\192.168.37.240\fire\防火材料\依頼試験"
    Private Const Nas2 As String = "\\192.168.37.241\fire"
    Private Const Nas3 As String = "\\192.168.37.242\fire"
    Private st As DateTime, ed As DateTime, time1 As TimeSpan
    Private Count1 As Long

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

        Using sfd As SaveFileDialog = New SaveFileDialog

            'デフォルトのファイル名を指定します
            sfd.FileName = TextBox3.Text
            sfd.InitialDirectory = Path2
            If sfd.ShowDialog() = DialogResult.OK Then
                Using sw As StreamWriter = New StreamWriter(sfd.FileName, False, System.Text.Encoding.UTF8)
                    'ファイルに書き込み
                    sw.Write(TextBox2.Text)
                End Using
            End If
        End Using
    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        TextBox1.Text = Nas1
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Label5.Text = Result.Length.ToString
        Dim ts As TimeSpan = DateTime.Now - st
        Label6.Text = ts.ToString()
        Label7.Text = Count1.ToString()
    End Sub

    Private Sub RadioButton2_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton2.CheckedChanged
        TextBox1.Text = Nas2
    End Sub

    Private Sub RadioButton3_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton3.CheckedChanged
        TextBox1.Text = Nas3
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim strHostName As String
        Dim ipAddr As IPAddress


        'ホスト名の取得
        strHostName = Dns.GetHostName()
        'ホスト名を表示
        'MessageBox.Show("ホスト名：" & strHostName, "処理結果")


        'IPリストの取得
        Dim adrList As IPAddress() = Dns.GetHostAddresses(strHostName)
        'IPリストの最初の値を取得
        ipAddr = adrList(1)
        ip = ipAddr.ToString()

        Result = ""
        'TextBox1.Text = "\\192.168.0.173\disk1\報告書（耐火）＿業務課から\2000Ⅲ耐火防火試験室"
        If TextBox1.Text = "" Then
            If ip.IndexOf(".37") = -1 Then
                TextBox1.Text = "\\192.168.0.173\disk1\報告書（耐火）＿業務課から\2000Ⅲ耐火防火試験室"
            Else
                TextBox1.Text = "\\192.168.37.241\fire\"
            End If

        End If
        TextBox2.Text = ""
        Path1 = TextBox1.Text
        Path2 = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\耐火NAS"
        Label1.Text = "操作１：ファイル情報を読み取るフォルダを選択する。"
        Label2.Text = "操作２：ファイル情報を読み取り、表示する"
        Label3.Text = "操作３：ファイル情報をテキストファイルに保存する。"

        RadioButton1.Text = "NAS1(192.168.37.240)"
        RadioButton2.Text = "NAS2(192.168.37.241)"
        RadioButton3.Text = "NAS3(192.168.37.242)"

        Label5.Text = "0"
        Label6.Text = "0"
        Label7.Text = "0"
        Timer1.Interval = 1000
        Timer1.Enabled = False

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'Dim sFolder As String = My.Computer.FileSystem.SpecialDirectories.MyDocuments
        'Result = ""
        st = DateTime.Now
        Timer1.Enabled = True
        Result = TextBox1.Text + vbCrLf + vbCrLf
        TextBox2.Text = ""
        TreeView1.ShowNodeToolTips = True
        TreeView1.BeginUpdate()
        TreeView1.Nodes.Clear()
        nn = 0
        AddNode(TreeView1.Nodes, Path1)
        TreeView1.EndUpdate()
        TextBox2.Text = Result
        ed = DateAndTime.Now
        time1 = ed - st
        TextBox2.Text += vbCrLf + time1.ToString + vbCrLf
        Timer1.Enabled = False
        Count1 = 0

        Dim i As Integer = 0
        Do
            i += 1
            If Path1.Substring(Path1.Length - i, 1) <> "　" And Path1.Substring(Path1.Length - i, 1) <> " " Then Exit Do
        Loop
        i -= 1
        If i > 0 Then
            Dim b As String = Path1.Substring(0, Path1.Length - i)
            Path1 = b
        End If
        'TextBox3.Text = Path.GetFileName(Path1) + ".txt"
        TextBox3.Text = Path1.Replace("\", "-") + ".txt"

    End Sub

    Sub AddNode(ByVal Nodes As TreeNodeCollection, ByVal sFolder As String)
        Dim N = Nodes.Add(System.IO.Path.GetFileName(sFolder))
        Dim Level As Integer = N.Level
        Dim tab1 As String
        Dim s As String
        If Level > 0 Then
            tab1 = New String(vbTab, Level) + "・ "
        Else
            tab1 = ""
        End If
        'Result += Path.GetFileName(Path.GetDirectoryName(sFolder)) + vbCrLf
        'Result += tab1 + Path.GetFileName(sFolder) + " (dir)" + vbCrLf
        'nn += 1
        'Try
        Dim i As Integer = 0
        Do
            i += 1
            If sFolder.Substring(sFolder.Length - i, 1) <> "　" And sFolder.Substring(sFolder.Length - i, 1) <> " " Then Exit Do
        Loop
        i -= 1
        'Dim a As String = sFolder.Substring(sFolder.Length - 1, 1)
        If i > 0 Then
            Dim b As String = sFolder.Substring(0, sFolder.Length - i)
            Dim c As String = sFolder + "a"
            System.IO.Directory.Move(sFolder, c)
            System.IO.Directory.Move(c, b)
            sFolder = b
        End If
        Result += tab1 + Path.GetFileName(sFolder) + " (dir)" + vbCrLf
        Count1 += 1
        's = sFolder + "\"
        'Directory.SetCurrentDirectory(s)
        'Dim dirpath As String = Directory.GetCurrentDirectory()
        Try
            For Each sName In My.Computer.FileSystem.GetDirectories(sFolder)
                'For Each sName In My.Computer.FileSystem.GetDirectories(Environment.CurrentDirectory)
                'For Each sName In Environment.CurrentDirectory.
                Application.DoEvents()
                AddNode(N.Nodes, sName)
                'Result += Path.GetFileName(Path.GetDirectoryName(sName)) + vbCrLf
            Next
        Catch ex As UnauthorizedAccessException
            N.ToolTipText = ex.Message
            N.ForeColor = Color.Red
            'Result += Path.GetFileName(Path.GetDirectoryName(sFolder)) + vbCrLf
            'nn -= 1
        End Try
        Try
            'Result += Path.GetFileName(Path.GetDirectoryName(sFolder)) + vbCrLf

            For Each sName In My.Computer.FileSystem.GetFiles(sFolder)
                Dim fname As String = Path.GetFileName(sName)
                'Result += fname + vbCrLf
                'nn += 1
                N.Nodes.Add(fname)
                Level = N.Level
                'If Level > 0 Then
                tab1 = New String(vbTab, Level + 1) + "・ "
                'Else
                'tab1 = ""
                'End If
                Result += tab1 + fname + " (file)" + vbCrLf
                Count1 += 1
            Next
        Catch ex As UnauthorizedAccessException
            N.ToolTipText = ex.Message
            N.ForeColor = Color.Red
            'Result += Path.GetFileName(Path.GetDirectoryName(sFolder)) + vbCrLf
            'nn -= 1
        End Try
        N.EnsureVisible()

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim fbd As New FolderBrowserDialog

        '上部に表示する説明テキストを指定する
        fbd.Description = "読み込むフォルダを指定してください。"
        'ルートフォルダを指定する
        'デフォルトでDesktop
        fbd.RootFolder = Environment.SpecialFolder.Desktop
        '最初に選択するフォルダを指定する
        'RootFolder以下にあるフォルダである必要がある
        'fbd.SelectedPath = "\\192.168.0.173\disk1\報告書（耐火）＿業務課から"
        If TextBox1.Text <> "" Then
            fbd.SelectedPath = TextBox1.Text
        Else
            fbd.SelectedPath = "\\192.168.37.241\fire\"
        End If

        'ユーザーが新しいフォルダを作成できるようにする
        'デフォルトでTrue
        fbd.ShowNewFolderButton = True

        'ダイアログを表示する
        If fbd.ShowDialog(Me) = DialogResult.OK Then
            '選択されたフォルダを表示する
            Me.TextBox1.Text = fbd.SelectedPath
            Path1 = fbd.SelectedPath
            ''Dim filename() As String, fname() As String, dir1() As String

            'Try
            '    filename = System.IO.Directory.GetFiles _
            '    (Path1, "*.xdw", System.IO.SearchOption.AllDirectories)
            '    Dim n As Integer = filename.Length
            '    Me.TextBox3.Text = ""

            '    ReDim fname(n - 1), dir1(n - 1)

            '    For i As Integer = 0 To n - 1
            '        fname(i) = System.IO.Path.GetFileNameWithoutExtension(filename(i))
            '        dir1(i) = System.IO.Path.GetFileName(System.IO.Path.GetDirectoryName(filename(i)))
            '        Me.TextBox3.Text += filename(i) + vbCrLf
            '        Me.TextBox3.SelectionStart = Me.TextBox3.Text.Length
            '        Me.TextBox3.Focus()
            '        Me.TextBox3.ScrollToCaret()
            '        'Console.WriteLine(a)

            '    Next
            'Catch e1 As Exception
            '    'Console.WriteLine(e1.Message)
            'End Try
        End If
    End Sub
End Class
