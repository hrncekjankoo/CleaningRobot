# Cleaning Robot application

To build and run the sample, go to *\ConsoleApp2\ConsoleApp2\bin\Release\net6.0\win-x64* directory and execute the following command:

`ConsoleApp2.exe {inputFilename} {outputFilename}` where inputFilename is file name of input json and outputFilename is output json.

```console
ConsoleApp2.exe test1.json result.json
```

# Run Robot in docker
Simply go to application main folder and build the docker image:
```console
docker build -t cleaner-robot -f Dockerfile .
```

It will create a docker image and copy testing files to docker application directory. Then just run the docker and it will return result and create it in docker application directory.
```console
docker run -d cleaner-robot test1.json result.json
```
