# Compress/Decompress Azure Function

This Azure Function will compress a payload POSTed to it using gzip, then base64 encode the compressed value and return it as the body of a 200 OK response.

## Example Compress call

~~~
POST /api/Compress HTTP/1.1
Host: localhost:7071
Content-Type: text/plain
Accept: */*
Cache-Control: no-cache
Host: localhost:7071
Accept-Encoding: gzip, deflate
Content-Length: 19
Connection: keep-alive
cache-control: no-cache

compress my string!
~~~

Returns 200 OK:

~~~
H4sIAAAAAAAAC0vOzy0oSi0uVsitVCguKcrMS1cEAGyldeQTAAAA
~~~

## Example Decompress call

~~~
POST /api/Decompress HTTP/1.1
Host: localhost:7071
Content-Type: text/plain
Accept: */*
Cache-Control: no-cache
Host: localhost:7071
Accept-Encoding: gzip, deflate
Content-Length: 52
Connection: keep-alive
cache-control: no-cache

H4sIAAAAAAAAC0vOzy0oSi0uVsitVCguKcrMS1cEAGyldeQTAAAA
~~~

Returns 200 OK:

~~~
compress my string!
~~~

## Unit Tests

The xUnit-based UTs in this project exercise the `Compress` Function, the `Decompress` Function, and ensure a round-trip results in a match to the initial input. Additionally, round-trip testing is done with known "problematic" international characters to ensure the encode/decode doesn't break things.