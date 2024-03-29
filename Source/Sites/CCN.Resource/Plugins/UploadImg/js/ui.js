/*global plupload */
/*global qiniu */
function FileProgress(file, targetID) {
    this.fileProgressID = file.id;
    this.file = file;

    this.opacity = 100;
    this.height = 0;
    this.fileProgressWrapper = $('#' + this.fileProgressID);
    if (!this.fileProgressWrapper.length) {
        previewImage(file, function (imgsrc) {
            //获取随机数
            strradom = (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);

            var strHtml = '<div id="' + strradom + '" class="imgPreview div2" >';
            strHtml +='<img data-type="img" src="' + imgsrc + '" class="loading">';
            strHtml +='<div class="caidan">';
            //strHtml += '<div hash="' + strradom + '" class="cz_1"><div class="czl_icon"></div></div>';
            //strHtml += '<div hash="' + strradom + '" class="cz_2"><div class="czl_icon"></div></div>';
            //strHtml += '<div hash="' + strradom + '" class="cz_3"><div class="czl_icon"></div></div>';
            strHtml += '<div hash="' + strradom + '" class="cz_4" id="' + file.id + '"><div class="czl_icon"></div></div>';
            strHtml += '</div></div>';
            $('.imgs').append(strHtml);

            // 立即执行事件绑定
            (function () {
                // 当前方法对象
                var fun = arguments.callee;
                // 图片选中
                $(".imgs .imgPreview").unbind().mousemove(function () {
                    $(this).find(".caidan").show();
                }).mouseout(function () {
                    $(this).find(".caidan").hide();
                });
                $(".imgs .caidan").unbind().mousemove(function () {
                    $(this).show();
                }).mouseout(function () {
                    $(this).hide();
                });
                // 图片删除
                $(".imgs .imgPreview .cz_4").unbind().click(function () {
                    $("#" + $(this).attr("hash")).remove();
                    var toremove = '';
                    var id = $(this).attr("data-val");
                    for (var i in uploader.files) {
                        if (uploader.files[i].id === id) {
                            toremove = i;
                        }
                    }
                    uploader.files.splice(toremove, 1);
                });
                // 置顶图片
                $(".imgs .imgPreview .cz_1").unbind().click(function () {
                    var imghtml = $("#" + $(this).attr("hash"))[0];
                    $("#" + $(this).attr("hash")).remove();
                    $('.imgs').prepend(imghtml);
                    fun();
                });
                // 图片前移
                $(".imgs .imgPreview .cz_2").unbind().click(function () {
                    var id = "#" + $(this).attr("hash");
                    var $this = $(id).prev();
                    var imghtml = $(id)[0];
                    if ($this.size()) {
                        $(id).remove();
                        $this.before(imghtml);
                        fun();
                    }
                });
                // 图片后移
                $(".imgs .imgPreview .cz_3").unbind().click(function () {
                    var id = "#" + $(this).attr("hash");
                    var $this = $(id).next();
                    var imghtml = $(id)[0];
                    if ($this.size()) {
                        $(id).remove();
                        $this.after(imghtml);
                        fun();
                    }
                });
            })();

        });
    } else {
        this.reset();
    }
}
$(document).on('click', '.pic_list a.pic_delete', function () {
    $(this).parent().parent().parent().remove();
    var toremove = '';
    var id = $(this).attr("data-val");
    for (var i in uploader.files) {
        if (uploader.files[i].id === id) {
            toremove = i;
        }
    }
    uploader.files.splice(toremove, 1);
});
//plupload中为我们提供了mOxie对象
//有关mOxie的介绍和说明请看：https://github.com/moxiecode/moxie/wiki/API
//如果你不想了解那么多的话，那就照抄本示例的代码来得到预览的图片吧
function previewImage(file, callback) {//file为plupload事件监听函数参数中的file对象,callback为预览图片准备完成的回调函数
    if (!file || !/image\//.test(file.type)) return; //确保文件是图片
    if (file.type == 'image/gif') {//gif使用FileReader进行预览,因为mOxie.Image只支持jpg和png
        var fr = new mOxie.FileReader();
        fr.onload = function () {
            callback(fr.result);
            fr.destroy();
            fr = null;
        }
        fr.readAsDataURL(file.getSource());
    } else {
        var preloader = new mOxie.Image();
        preloader.onload = function () {
            //preloader.downsize(550, 400);//先压缩一下要预览的图片,宽300，高300
            var imgsrc = preloader.type == 'image/jpeg' ? preloader.getAsDataURL('image/jpeg', 80) : preloader.getAsDataURL(); //得到图片src,实质为一个base64编码的数据
            callback && callback(imgsrc); //callback传入的参数为预览图片的url
            preloader.destroy();
            preloader = null;
        };
        preloader.load(file.getSource());
    }
}

FileProgress.prototype.setTimer = function (timer) {
    this.fileProgressWrapper.FP_TIMER = timer;
};

FileProgress.prototype.getTimer = function (timer) {
    return this.fileProgressWrapper.FP_TIMER || null;
};

FileProgress.prototype.reset = function () {
    this.fileProgressWrapper.attr('class', "progressContainer");
    this.fileProgressWrapper.find('td .progress .progress-bar-info').attr('aria-valuenow', 0).width('0%').find('span').text('');
    this.appear();
};

FileProgress.prototype.setChunkProgess = function (chunk_size) {
    var chunk_amount = Math.ceil(this.file.size / chunk_size);
    if (chunk_amount === 1) {
        return false;
    }

    var viewProgess = $('<button class="btn btn-default">查看分块上传进度</button>');

    var progressBarChunkTr = $('<tr class="chunk-status-tr"><td colspan=3></td></tr>');
    var progressBarChunk = $('<div/>');
    for (var i = 1; i <= chunk_amount; i++) {
        var col = $('<div class="col-md-2"/>');
        var progressBarWrapper = $('<div class="progress progress-striped"></div');

        var progressBar = $("<div/>");
        progressBar.addClass("progress-bar progress-bar-info text-left")
            .attr('role', 'progressbar')
            .attr('aria-valuemax', 100)
            .attr('aria-valuenow', 0)
            .attr('aria-valuein', 0)
            .width('0%')
            .attr('id', this.file.id + '_' + i)
            .text('');

        var progressBarStatus = $('<span/>');
        progressBarStatus.addClass('chunk-status').text();

        progressBarWrapper.append(progressBar);
        progressBarWrapper.append(progressBarStatus);

        col.append(progressBarWrapper);
        progressBarChunk.append(col);
    }

    if (!this.fileProgressWrapper.find('td:eq(2) .btn-default').length) {
        this.fileProgressWrapper.find('td>div').append(viewProgess);
    }
    progressBarChunkTr.hide().find('td').append(progressBarChunk);
    progressBarChunkTr.insertAfter(this.fileProgressWrapper);

};

FileProgress.prototype.setProgress = function (percentage, speed, chunk_size) {
    this.fileProgressWrapper.attr('class', "progressContainer green");

    var file = this.file;
    var uploaded = file.loaded;

    var size = plupload.formatSize(uploaded).toUpperCase();
    var formatSpeed = plupload.formatSize(speed).toUpperCase();
    var progressbar = this.fileProgressWrapper.find('td .progress').find('.progress-bar-info');
    if (this.fileProgressWrapper.find('.status').text() === '取消上传') {
        return;
    }
    this.fileProgressWrapper.find('.status').text("已上传: " + size + " 上传速度： " + formatSpeed + "/s");
    percentage = parseInt(percentage, 10);
    if (file.status !== plupload.DONE && percentage === 100) {
        percentage = 99;
    }

    progressbar.attr('aria-valuenow', percentage).css('width', percentage + '%');

    if (chunk_size) {
        var chunk_amount = Math.ceil(file.size / chunk_size);
        if (chunk_amount === 1) {
            return false;
        }
        var current_uploading_chunk = Math.ceil(uploaded / chunk_size);
        var pre_chunk, text;

        for (var index = 0; index < current_uploading_chunk; index++) {
            pre_chunk = $('#' + file.id + "_" + index);
            pre_chunk.width('100%').removeClass().addClass('alert-success').attr('aria-valuenow', 100);
            text = "块" + index + "上传进度100%";
            pre_chunk.next().html(text);
        }

        var currentProgessBar = $('#' + file.id + "_" + current_uploading_chunk);
        var current_chunk_percent;
        if (current_uploading_chunk < chunk_amount) {
            if (uploaded % chunk_size) {
                current_chunk_percent = ((uploaded % chunk_size) / chunk_size * 100).toFixed(2);
            } else {
                current_chunk_percent = 100;
                currentProgessBar.removeClass().addClass('alert-success');
            }
        } else {
            var last_chunk_size = file.size - chunk_size * (chunk_amount - 1);
            var left_file_size = file.size - uploaded;
            if (left_file_size % last_chunk_size) {
                current_chunk_percent = ((uploaded % chunk_size) / last_chunk_size * 100).toFixed(2);
            } else {
                current_chunk_percent = 100;
                currentProgessBar.removeClass().addClass('alert-success');
            }
        }
        currentProgessBar.width(current_chunk_percent + '%');
        currentProgessBar.attr('aria-valuenow', current_chunk_percent);
        text = "块" + current_uploading_chunk + "上传进度" + current_chunk_percent + '%';
        currentProgessBar.next().html(text);
    }

    this.appear();
};

FileProgress.prototype.setComplete = function (up, info) {
    var td = this.fileProgressWrapper.find('td:eq(2)'),
        tdProgress = td.find('.progress');

    var res = $.parseJSON(info);
    var url;
    if (res.url) {
        url = res.url;
        str = "<div><strong>Link:</strong><a href=" + res.url + " target='_blank' > " + res.url + "</a></div>" +
            "<div class=hash><strong>Hash:</strong>" + res.hash + "</div>";
    } else {
        var domain = up.getOption('domain');
        url = domain + encodeURI(res.key);
        var link = domain + res.key;
        str = "<div><strong>Link:</strong><a href=" + url + " target='_blank' > " + link + "</a></div>" +
            "<div class=hash><strong>Hash:</strong>" + res.hash + "</div>";
    }

    tdProgress.html(str).removeClass().next().next('.status').hide();
    td.find('.progressCancel').hide();

    var progressNameTd = this.fileProgressWrapper.find('.progressName');
    var imageView = '?imageView2/1/w/100/h/100';

    var isImage = function (url) {
        var res, suffix = "";
        var imageSuffixes = ["png", "jpg", "jpeg", "gif", "bmp"];
        var suffixMatch = /\.([a-zA-Z0-9]+)(\?|\@|$)/;

        if (!url || !suffixMatch.test(url)) {
            return false;
        }
        res = suffixMatch.exec(url);
        suffix = res[1].toLowerCase();
        for (var i = 0, l = imageSuffixes.length; i < l; i++) {
            if (suffix === imageSuffixes[i]) {
                return true;
            }
        }
        return false;
    };

    var isImg = isImage(url);

    var Wrapper = $('<div class="Wrapper"/>');
    var imgWrapper = $('<div class="imgWrapper col-md-3"/>');
    var linkWrapper = $('<a class="linkWrapper" target="_blank"/>');
    //var showImg = $('<img src="loading.gif"/>');
    var showImg = $('<img src=""/>');
    progressNameTd.append(Wrapper);

    if (!isImg) {
        showImg.attr('src', 'default.png');
        Wrapper.addClass('default');

        imgWrapper.append(showImg);
        Wrapper.append(imgWrapper);
    } else {
        linkWrapper.append(showImg);
        imgWrapper.append(linkWrapper);
        Wrapper.append(imgWrapper);

        var img = new Image();
        if (!/imageView/.test(url)) {
            url += imageView
        }
        $(img).attr('src', url);

        var height_space = 340;
        $(img).on('load', function () {
            showImg.attr('src', url);

            linkWrapper.attr('href', url).attr('title', '查看原图');

            function initImg(url, key, height) {
                $('#myModal-img').modal();
                var modalBody = $('#myModal-img').find('.modal-body');
                if (height <= 300) {
                    $('#myModal-img').find('.text-warning').show();
                }
                var newImg = new Image();
                modalBody.find('img').attr('src', 'loading.gif');
                newImg.onload = function () {
                    modalBody.find('img').attr('src', url).data('key', key).data('h', height);
                    modalBody.find('.modal-body-wrapper').find('a').attr('href', url);
                };
                newImg.src = url;
            }

            var infoWrapper = $('<div class="infoWrapper col-md-6"></div>');


            //var fopLink = $('<a class="fopLink"/>');
            //fopLink.attr('data-key', res.key).text('查看处理效果');
            //infoWrapper.append(fopLink);
            //fopLink.on('click', function() {
            //    var key = $(this).data('key');
            //    var height = parseInt($(this).parents('.Wrapper').find('.origin-height').text(), 10);
            //    if (height > $(window).height() - height_space) {
            //        height = parseInt($(window).height() - height_space, 10);
            //    } else {
            //        height = parseInt(height, 10) || 300;
            //        //set a default height 300 for ie9-
            //    }
            //    var fopArr = [];
            //    fopArr.push({
            //        fop: 'imageView2',
            //        mode: 3,
            //        h: height,
            //        q: 100,
            //        format: 'png'
            //    });
            //    fopArr.push({
            //        fop: 'watermark',
            //        mode: 1,
            //        image: 'http://www.b1.qiniudn.com/images/logo-2.png',
            //        dissolve: 100,
            //        gravity: 'SouthEast',
            //        dx: 100,
            //        dy: 100
            //    });
            //    var url = Qiniu.pipeline(fopArr, key);
            //    $('#myModal-img').on('hide.bs.modal', function() {
            //        $('#myModal-img').find('.btn-default').removeClass('disabled');
            //        $('#myModal-img').find('.text-warning').hide();
            //    }).on('show.bs.modal', function() {
            //        $('#myModal-img').find('.imageView').find('a:eq(0)').addClass('disabled');
            //        $('#myModal-img').find('.watermark').find('a:eq(3)').addClass('disabled');
            //        $('#myModal-img').find('.text-warning').hide();
            //    });

            //    initImg(url, key, height);

            //    return false;
            //});

            var ie = Qiniu.detectIEVersion();
            if (!(ie && ie <= 9)) {
                //var exif = Qiniu.exif(res.key);
                //if (exif) {
                //    var exifLink = $('<a href="" target="_blank">查看exif</a>');
                //    exifLink.attr('href', url + '?exif');
                //    infoWrapper.append(exifLink);
                //}

                var imageInfo = Qiniu.imageInfo(res.key);
                var infoArea = $('<div/>');
                //var infoInner = '<div>格式：<span class="origin-format">' + imageInfo.format + '</span></div>' +
                //    '<div>宽度：<span class="orgin-width">' + imageInfo.width + 'px</span></div>' +
                //    '<div>高度：<span class="origin-height">' + imageInfo.height + 'px</span></div>';
                //infoArea.html(infoInner);

                infoWrapper.append(infoArea);
            }

            Wrapper.append(infoWrapper);

        }).on('error', function () {
            showImg.attr('src', 'default.png');
            Wrapper.addClass('default');
        });
    }
};
FileProgress.prototype.setError = function () {
    this.fileProgressWrapper.find('td:eq(2)').attr('class', 'text-warning');
    this.fileProgressWrapper.find('td:eq(2) .progress').css('width', 0).hide();
    this.fileProgressWrapper.find('button').hide();
    this.fileProgressWrapper.next('.chunk-status-tr').hide();
};

FileProgress.prototype.setCancelled = function (manual) {
    var progressContainer = 'progressContainer';
    if (!manual) {
        progressContainer += ' red';
    }
    this.fileProgressWrapper.attr('class', progressContainer);
    this.fileProgressWrapper.find('td .progress').remove();
    this.fileProgressWrapper.find('td:eq(2) .btn-default').hide();
    this.fileProgressWrapper.find('td:eq(2) .progressCancel').hide();
};

FileProgress.prototype.setStatus = function (status, isUploading) {
    if (!isUploading) {
        this.fileProgressWrapper.find('.status').text(status).attr('class', 'status text-left');
    }
};

// 绑定取消上传事件
FileProgress.prototype.bindUploadCancel = function (up) {
    var self = this;
    if (up) {
        self.fileProgressWrapper.find('td:eq(2) .progressCancel').on('click', function () {
            self.setCancelled(false);
            self.setStatus("取消上传");
            self.fileProgressWrapper.find('.status').css('left', '0');
            up.removeFile(self.file);
        });
    }

};

FileProgress.prototype.appear = function () {
    if (this.getTimer() !== null) {
        clearTimeout(this.getTimer());
        this.setTimer(null);
    }

    if (this.fileProgressWrapper[0].filters) {
        try {
            this.fileProgressWrapper[0].filters.item("DXImageTransform.Microsoft.Alpha").opacity = 100;
        } catch (e) {
            // If it is not set initially, the browser will throw an error.  This will set it if it is not set yet.
            this.fileProgressWrapper.css('filter', "progid:DXImageTransform.Microsoft.Alpha(opacity=100)");
        }
    } else {
        this.fileProgressWrapper.css('opacity', 1);
    }

    this.fileProgressWrapper.css('height', '');

    this.height = this.fileProgressWrapper.offset().top;
    this.opacity = 100;
    this.fileProgressWrapper.show();

};
