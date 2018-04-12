/// <binding AfterBuild='Run - Development' />

var fs = require('fs');
var configuration = fs.readFileSync("BuildConfiguration.txt", 'ascii').split(/[\n\r]/)[0];

if (configuration === "Debug") {
    module.exports = require('./webpack.development.config.js');
} else {
    module.exports = require('./webpack.production.config');
}

//TODO: a project pre-build eventjébe bele kell tenni: 
//echo $(ConfigurationName)>"$(ProjectDir)BuildConfiguration.txt"
