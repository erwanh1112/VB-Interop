Module VBApp
    ' See definition of FILETIME on https://docs.microsoft.com/en-us/windows/win32/api/minwinbase/ns-minwinbase-filetime
    <Runtime.InteropServices.StructLayoutAttribute(
     Runtime.InteropServices.LayoutKind.Sequential)>
    Public Structure FILETIME
        Public dwLowDateTime As UInteger   ' DWORD
        Public dwHighDateTime As UInteger  ' DWORD
    End Structure


    ' See definition of WIN32_FIND_DATAW on https://docs.microsoft.com/en-us/windows/win32/api/minwinbase/ns-minwinbase-win32_find_dataw
    <Runtime.InteropServices.StructLayout(
     Runtime.InteropServices.LayoutKind.Sequential,
     CharSet:=Runtime.InteropServices.CharSet.Unicode)>
    Public Structure WIN32_FIND_DATAW
        Public dwFileAttributes As UInteger ' DWORD
        Public ftCreationTime As FILETIME
        Public ftLastAccessTime As FILETIME
        Public ftLastWriteTime As FILETIME
        Public nFileSizeHigh As UInteger    ' DWORD
        Public nFileSizeLow As UInteger     ' DWORD
        Public dwReserved0 As UInteger      ' DWORD
        Public dwReserved1 As UInteger      ' DWORD
        <Runtime.InteropServices.MarshalAs(
         Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=260)>
        Public cFileName As String          ' WCHAR[260]
        <Runtime.InteropServices.MarshalAs(
         Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=14)>
        Public cAlternateFileName As String ' WCHAR[14]
        Public dwFileType As UInteger       ' DWORD
        Public dwCreatorType As UInteger    ' DWORD
        Public wFniderFlag As UShort        ' WORD
    End Structure

    Public Class NativeFunctions

        ' See definition of FindFirstFileW() on https://docs.microsoft.com/en-us/windows/win32/api/fileapi/nf-fileapi-findfirstfilew
        <Runtime.InteropServices.DllImport("kernel32.dll", EntryPoint:="FindFirstFileW")>
        Public Shared Function FindFirstFileW(
            <Runtime.InteropServices.MarshalAs(Runtime.InteropServices.UnmanagedType.LPWStr)>
            ByVal lpFileName As String,              ' LPCWSTR
            ByRef lpFindFileData As WIN32_FIND_DATAW ' LPWIN32_FIND_DATAW
                                              ) As IntPtr ' HANDLE
        End Function

        <Runtime.InteropServices.DllImport("kernel32.dll", EntryPoint:="FindClose")>
        Public Shared Function FindClose(
            ByVal hFindFile As IntPtr         ' HANDLE
                                        ) As Boolean ' BOOL
        End Function

        <Runtime.InteropServices.DllImport("ServerInterop.dll", EntryPoint:="GetFirstLogType", CharSet:=Runtime.InteropServices.CharSet.Ansi)>
        Public Shared Function GetFirstLogType(
            ByRef blob As Integer,             ' int*
            ByVal key As Text.StringBuilder,   ' char*
            ByRef keySize As Integer,          ' int*
            ByVal value As Text.StringBuilder, ' char*
            ByRef valueSize As Integer         ' int*
                                              ) As Boolean ' bool
        End Function

        <Runtime.InteropServices.DllImport("ServerInterop.dll", EntryPoint:="GetNextLogType", CharSet:=Runtime.InteropServices.CharSet.Ansi)>
        Public Shared Function GetNextLogType(
            ByRef blob As Integer,             ' int*
            ByVal key As Text.StringBuilder,   ' char*
            ByRef keySize As Integer,          ' int*
            ByVal value As Text.StringBuilder, ' char*
            ByRef valueSize As Integer         ' int*
                                              ) As Boolean ' bool
        End Function
    End Class

    Sub Main()
        ' Call into Win32 APIs.
        Console.WriteLine("Calling Win32 API FindFirstFileW()...")
        Dim fileName As String = "VBApp.exe" ' File name to search.
        Dim findData As WIN32_FIND_DATAW = New WIN32_FIND_DATAW()
        Dim hFind As IntPtr = NativeFunctions.FindFirstFileW(fileName, findData)

        If hFind = -1 Then
            Console.WriteLine($"File {fileName} not found")
            NativeFunctions.FindClose(hFind)
        Else
            Console.WriteLine($"Found file {fileName}!")
        End If

        ' Call into ServerInterop APIs.
        Console.WriteLine()
        Console.WriteLine("Calling ServerInterop APIs Get*LogTypes()...")
        Dim blob As Integer
        Dim keySize As Integer = 260
        Dim key As Text.StringBuilder = New Text.StringBuilder(keySize)
        Dim valueSize As Integer = 260
        Dim value As Text.StringBuilder = New Text.StringBuilder(valueSize)
        Dim found As Boolean = NativeFunctions.GetFirstLogType(blob, key, keySize, value, valueSize)

        If Not found Then
            Console.WriteLine($"Failed to retrieve log types!")
        End If

        Do While found
            Console.WriteLine($"Found {key.ToString()}={value.ToString()}")
            keySize = 260
            valueSize = 260
            found = NativeFunctions.GetNextLogType(blob, key, keySize, value, valueSize)
        Loop

        Console.WriteLine()
        Console.WriteLine("Press ENTER to exit...")
        Console.ReadKey()
    End Sub

End Module
