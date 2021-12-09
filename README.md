# Max_Parser

$Max-Parser was designed and created for all DFIR analysts who want to check the maximum size of the $UsnJrnl. That information can be retrived from $Max file.

Based on this article https://docs.microsoft.com/en-us/windows/win32/intl/using-byte-order-marks we know that "Microsoft uses UTF-16, little endian byte order." What does it mean for us? Well, if we want to read as let's say standard "string" we have to reverse the order of bytes. For more information regarding "Big-endian" and "Little-endian" you can check the Wikipedia, which explains the diffrences https://en.wikipedia.org/wiki/Endianness.

byte[] ByteContentOfMax = System.IO.File.ReadAllBytes(pathToMax);
                        string ContentOfMax = BitConverter.ToString(ByteContentOfMax);

                        if (ByteContentOfMax.Length != 32)
                        {
                            MessageBox.Show("The size of the provided file does not match the size of the $Max file, please try again.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                        else
                        {
                            byte[] ByteUsnJrnlSize = new byte[8];
                            for (int i = 0; i < 8; i++)
                            {
                                ByteUsnJrnlSize[i] = ByteContentOfMax[i];
                            }

                            Array.Reverse(ByteUsnJrnlSize);
                            string StringReverseUsnJrnlSize = BitConverter.ToString(ByteUsnJrnlSize);
                            long decValue = long.Parse(StringReverseUsnJrnlSize.Replace("-", ""), System.Globalization.NumberStyles.HexNumber);

                            textBoxContent.Text = ContentOfMax.Replace('-', ' ');
                            textBoxMaxSizeMB.Text = GetMBfromBytes(decValue) + " MB";
                            textBoxMaxSizeGB.Text = GetGBfromBytes(decValue) + " GB";
                        }

