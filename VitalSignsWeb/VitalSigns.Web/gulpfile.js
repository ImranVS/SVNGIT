/// <binding AfterBuild='views' />

var gulp = require('gulp');
var del = require('del');

gulp.task('clean:angular', function () {
    return del('wwwroot/angular2/**/*');
});

gulp.task("clean", ["clean:libs", "clean:angular"]);

gulp.task('copy:libs', function () {
    return gulp.src([
        'node_modules/core-js/client/shim.min.js',
        'node_modules/core-js/client/shim.min.js.map',
        'node_modules/zone.js/dist/zone.js',
        'node_modules/reflect-metadata/Reflect.js',
        'node_modules/reflect-metadata/Reflect.js.map',
        'node_modules/systemjs/dist/system.src.js'
    ]).pipe(gulp.dest('wwwroot/angular2'))
});

gulp.task('copy:angular', function () {
    return gulp.src([
        'node_modules/@angular/**/*.js',
        'node_modules/@angular/**/*.js.map',
    ]).pipe(gulp.dest('wwwroot/angular2/@angular'))
});

gulp.task('copy:rxjs', function () {
    return gulp.src([
        'node_modules/rxjs/**/*.js',
        'node_modules/rxjs/**/*.js.map',
    ]).pipe(gulp.dest('wwwroot/angular2/rxjs'))
});

gulp.task('copy:angular2inmemorywebapi', function () {
    return gulp.src([
        'node_modules/angular2-in-memory-web-api/**/*.js',
        'node_modules/angular2-in-memory-web-api/**/*.js.map',
    ]).pipe(gulp.dest('wwwroot/angular2/angular2-in-memory-web-api'))
});

gulp.task('copy:dragula', function () {
    return gulp.src([
        'node_modules/dragula/dist/**/*.*',
    ]).pipe(gulp.dest('wwwroot/lib/dragula'))
});

gulp.task('copy:ng2-dragula', function () {
    return gulp.src([
        'node_modules/ng2-dragula/**/*',
    ]).pipe(gulp.dest('wwwroot/lib/ng2-dragula'))
});

gulp.task("copy", ["copy:libs", "copy:angular", "copy:rxjs", "copy:angular2inmemorywebapi"]);

gulp.task('views', function () {
    return gulp.src('./scripts/**/*.html').pipe(gulp.dest('./wwwroot/app/'));
});