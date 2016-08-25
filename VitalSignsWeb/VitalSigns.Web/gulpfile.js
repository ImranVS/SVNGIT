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
        'node_modules/zone.js/dist/zone.js',
        'node_modules/reflect-metadata/Reflect.js',
        'node_modules/systemjs/dist/system.src.js'
    ]).pipe(gulp.dest('wwwroot/angular2'))
});

gulp.task('copy:angular', function () {
    return gulp.src([
        'node_modules/@angular/**/*.js',
    ]).pipe(gulp.dest('wwwroot/angular2/@angular'))
});

gulp.task('copy:rxjs', function () {
    return gulp.src([
        'node_modules/rxjs/**/*.js',
    ]).pipe(gulp.dest('wwwroot/angular2/rxjs'))
});

gulp.task('copy:angular2inmemorywebapi', function () {
    return gulp.src([
        'node_modules/angular2-in-memory-web-api/**/*.js',
    ]).pipe(gulp.dest('wwwroot/angular2/angular2-in-memory-web-api'))
});

gulp.task("copy", ["copy:libs", "copy:angular", "copy:rxjs", "copy:angular2inmemorywebapi"]);

gulp.task('views', function () {
    return gulp.src('./scripts/**/*.html').pipe(gulp.dest('./wwwroot/app/'));
});