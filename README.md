# Resub #

It's a simple console tool to adjust the time offset of the subtitles found in SubRip(.srt) files.

**Command line invocation:**

`resub.exe [inputfile_options] -i inputfile -offset offsetvalue [outputfile_options] -o outputfile`

However, there are no options to set yet.

For example if you want to display your subtitle text 2 seconds earlier than originally, you could use:

`resub.exe -i "d:\movies\sample.srt" -offset -2s -o "d:\movies\sample_adjusted.srt"`

**Offset value**

To set the offset value you can use human readable format, eg.:
`+1h5m10s` or 
`-1h5m10s` or
`1200ms` or
`-1m2s10ms` and so on, you get the gist of it...

The program is doing a few sanity checks on the input file, for example the subtitle file must be a well-formed .srt file.