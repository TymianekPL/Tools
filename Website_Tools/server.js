const http = require("http");
const url = require("url");
const fs = require("fs");
const mime = require("mime");
const PORT = 1000;
console.clear();

http.createServer(function (req, res) {
     const q = url.parse(req.url, true);
     let filename = "./src" + q.pathname;
     try {
          StartHttp();
     } catch {
          console.log("Error!");
          console.log("Connection failed!");
     }
     function StartHttp() {
          fs.readFile(filename, function (err, data) {
               if (err) {
                    if (filename.endsWith("/")) {
                         filename = filename + "index.html";
                         StartHttp();
                         return;
                    }
                    res.writeHead(404, { "Content-Type": "text/html;charset=utf-8" });
                    return res.end(
                         "404 Not Found\n<br /><code>" +
                              q.pathname +
                              "</code> not found"
                    );
               }
               let contentType = mime.getType(filename);
               res.writeHead(200, {"Content-Type": `${contentType};charset=utf-8` });
               res.write(data);
               return res.end();
          });
     }
}).listen(PORT, "0.0.0.0", 5, () => {
     console.log(`Server is listening on http://0.0.0.0:${PORT}/`);
});
