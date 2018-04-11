var environment = (process.env.NODE_ENV || "production").trim();
if (environment === "development") {
    module.exports = require('./webpack.development.config.js');
} else {
    module.exports = require('./webpack.production.config');
}
