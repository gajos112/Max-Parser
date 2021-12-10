# $Max-Parser

$Max-Parser was designed and created for all DFIR analysts who want to check the maximum size of the $UsnJrnl. That information can be retrived from $Max file.

Like my other tools, it is a GUI tool written in C# .Net Framework 4.7.2. It was tested on:

- Windows 10.0.16299,
- Windows 10.0.19042.

# How does it work?

![alt text](https://github.com/gajos112/Max-Parser/blob/main/Images/1.png?raw=true)

There are two ways to use that tool. The first option allows you to parse $Max file based on the path you provided. 

![alt text](https://github.com/gajos112/Max-Parser/blob/main/Images/2.png?raw=true)


Once the path is provided you have to click "Parse the file...". First it will check if the file indeed exists:
```
if (File.Exists(textBoxPathToMaxFile.Text)) if (File.Exists(textBoxPathToMaxFile.Text))
```

If the file exists, it reads all bytes frome the file to the array:
```
byte[] ByteContentOfMax = System.IO.File.ReadAllBytes(pathToMax);
string ContentOfMax = BitConverter.ToString(ByteContentOfMax);
```

Then it checks the lenght of the array, why? Becuase $Max is 32 bytes long:
```
if (ByteContentOfMax.Length != 32)
{
    MessageBox.Show("The size of the provided file does not match the size of the $Max file, please try again.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
}
```

If the length is fine, it takes first 8 bytes from the array:
```
else
{
    byte[] ByteUsnJrnlSize = new byte[8];
    for (int i = 0; i < 8; i++)
    {
        ByteUsnJrnlSize[i] = ByteContentOfMax[i];
    }
```

To understand wh we need first 8 bytes, we have to know the structure of that file, which is explained on this blog: http://forensicinsight.org/wp-content/uploads/2013/07/F-INSIGHT-Advanced-UsnJrnl-Forensics-English.pdf. 
Quick overview can be found below.

- Offset: 0x00, size: 8 bytes -> Maximum Size The maximum size of log data
- Offset: 0x08, size: 8 Allocation Size The size of allocated area when new log data is saved.
- Offset: 0x10, size: 8 USN ID The creation time of "$UsnJrnl" file(FILETIME)
- Offset: 0x18, size: 8 Lowest Valid USN The least value of USN in current records With this value, investigator can approach the start point of first record within "$J" attribute

When we have first 8 bytes, we how to reverse the order of bytes.
```
    Array.Reverse(ByteUsnJrnlSize);
```

Why reverse the order? Based on this article https://docs.microsoft.com/en-us/windows/win32/intl/using-byte-order-marks we know that "Microsoft uses UTF-16, little endian byte order." What does it mean for us? Well, if we want to read bytes as let's say a standard "string" we have to reverse the order of bytes. For more information regarding "Big-endian" and "Little-endian" you can check the Wikipedia, which explains the diffrences quite well https://en.wikipedia.org/wiki/Endianness.

Now the tool can convert hexadecimal to decimal value.
```
    string StringReverseUsnJrnlSize = BitConverter.ToString(ByteUsnJrnlSize);
    long decValue = long.Parse(StringReverseUsnJrnlSize.Replace("-", ""), System.Globalization.NumberStyles.HexNumber);
```

The size is stored in bytes, thefore I created two small functions that convert it to MB and GB.
```
    textBoxMaxSizeMB.Text = GetMBfromBytes(decValue) + " MB";
    textBoxMaxSizeGB.Text = GetGBfromBytes(decValue) + " GB";
```

In results you will get the size of the $UsnJrnl, what you can see in the screenshot below:
![alt text](https://github.com/gajos112/Max-Parser/blob/main/Images/3.png?raw=true)

![alt text](https://github.com/gajos112/Max-Parser/blob/main/Images/4.png?raw=true)
![alt text](https://github.com/gajos112/Max-Parser/blob/main/Images/5.png?raw=true)
![alt text](https://github.com/gajos112/Max-Parser/blob/main/Images/6.png?raw=true)
![alt text](https://github.com/gajos112/Max-Parser/blob/main/Images/7.png?raw=true)



 
 











