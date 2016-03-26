/// <binding Clean='clean' />
"use strict";

var gulp = require("gulp"),
    rimraf = require("rimraf"),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    filter = require("gulp-filter"),
    mainBowerFiles = require("gulp-main-bower-files"),
    print = require("gulp-print"),
    rename = require("gulp-rename"),
    uglify = require("gulp-uglify"),
    merge = require("merge-stream");

var paths = {
    webroot: "./wwwroot/"
};

paths.js = paths.webroot + "js/**/*.js";
paths.minJs = paths.webroot + "js/**/*.min.js";
paths.css = paths.webroot + "css/**/*.css";
paths.minCss = paths.webroot + "css/**/*.min.css";
paths.concatJsDest = paths.webroot + "js/site.min.js";
paths.concatCssDest = paths.webroot + "css/site.min.css";

gulp.task("clean:js", function (cb) {
    rimraf(paths.concatJsDest, cb);
});

gulp.task("clean:css", function (cb) {
    rimraf(paths.concatCssDest, cb);
});

gulp.task("clean", ["clean:js", "clean:css"]);

var mainBowerFilesOverrides = {
    overrides: {
        // Bootstrap's bower.json only specifies the JavaScript.
        bootstrap: {
            main: [
                './dist/js/bootstrap.js',
                './dist/css/*.css',
                './dist/fonts/*.*'
            ]
        }
    }
};

gulp.task('main-bower-files', function () {
    // This will create minified versions of all the library CSS/JS files. It
    // will not (yet) combine them. It will not persist any pre-minified files.

    var minifiedCSS = gulp.src('./bower.json')
        .pipe(mainBowerFiles(['**/*.css', '!**/*.min.css'], mainBowerFilesOverrides))
        .pipe(print(function (path) {
            return "MinifyCSS: " + path;
        }))
        .pipe(rename({
            extname: '.min.css'
        }))
        .pipe(print(function (path) {
            return "MiniCSS: " + path;
        }))
        .pipe(cssmin());

    var minifiedJS = gulp.src('./bower.json')
        .pipe(mainBowerFiles(['**/*.js', '!**/*.min.js'], mainBowerFilesOverrides))
        .pipe(print(function (path) {
            return "MinifyJS: " + path;
        }))
        .pipe(rename({
            extname: '.min.js'
        }))
        .pipe(print(function (path) {
            return "MiniJS: " + path;
        }))
        .pipe(uglify());

    var raw = gulp.src('./bower.json')
        .pipe(mainBowerFiles(['**/*.*', '!**/*.min.css', '!**/*.min.js'], mainBowerFilesOverrides))
        .pipe(print(function (path) {
            return "RAW: " + path;
        }))
        .pipe(gulp.dest(paths.webroot + 'lib/'));

    return merge(minifiedCSS, minifiedJS, raw)
        .pipe(gulp.dest(paths.webroot + 'lib/'))
        .pipe(print());
});

gulp.task("min:js", function () {
    return gulp.src([paths.js, "!" + paths.minJs], { base: "." })
        .pipe(concat(paths.concatJsDest))
        .pipe(uglify())
        .pipe(gulp.dest("."));
});

gulp.task("min:css", function () {
    return gulp.src([paths.css, "!" + paths.minCss])
        .pipe(concat(paths.concatCssDest))
        .pipe(cssmin())
        .pipe(gulp.dest("."));
});

gulp.task("min", ["min:js", "min:css"]);
