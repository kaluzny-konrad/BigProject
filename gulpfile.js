/// <binding AfterBuild='default' />
var gulp = require("gulp");
var uglify = require("gulp-uglify");
var cleanCss = require('gulp-clean-css');
var concat = require("gulp-concat");

// minify JS
gulp.task('pack-js', function () {
    return gulp.src(["wwwroot/js/**/*.js"])
        .pipe(uglify())
        .pipe(concat("duchtreat.min.js"))
        .pipe(gulp.dest('wwwroot/dist/'));
});

// minify CSS

gulp.task('pack-css', function () {
    return gulp.src(["wwwroot/css/**/*.css"])
        .pipe(cleanCss())
        .pipe(concat("duchtreat.min.css"))
        .pipe(gulp.dest('wwwroot/dist/'));
});

gulp.task('default', gulp.series('pack-js', 'pack-css'));