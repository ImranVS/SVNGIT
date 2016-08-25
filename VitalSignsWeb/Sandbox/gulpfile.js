/// <binding AfterBuild='build' Clean='clean' />


var gulp = require('gulp');
var rimraf = require('rimraf');

var paths = {
    npmSrc: "./node_modules/",
    libTarget: "./wwwroot/libs/"
};

var libsToMove = [
   paths.npmSrc + '/angular2/bundles/angular2-polyfills.js',
   paths.npmSrc + '/systemjs/dist/system.js',
   paths.npmSrc + '/systemjs/dist/system-polyfills.js',
   paths.npmSrc + '/rxjs/bundles/Rx.js',
   paths.npmSrc + '/angular2/bundles/angular2.dev.js',
   paths.npmSrc + '/es6-shim/es6-shim.min.js',
   paths.npmSrc + '/angular2/bundles/http.dev.js',
   paths.npmSrc + '/reflect-metadata/reflect.js' 
  
];

// run after build event, used to copy javascripts files from npm to libs folder
gulp.task('build', function () {
    return gulp.src(libsToMove).pipe(gulp.dest(paths.libTarget));
});

// run after clean event, used to clean libs folder
gulp.task('clean', function (callback) {
    rimraf(paths.libTarget, callback);  
})