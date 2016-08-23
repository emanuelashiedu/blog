//Method to create alert messages (Error message alert , Heaer notification alert)

var Alert = {
    createAlert: function () {
        $('body').append('<div id="lightbox" class="lightbox"><div class="message_box_outer"><div class="dialog_box"><div id="lightboxtitle_box" class=""><div id="lightboxtitle" class="title_line">Alert !</div></div><div class="content_box"><ul id="content_box"></ul></div><div class="msg_footer"><a href="javascript:;" id="lightboxclose" onclick="Alert.hideAlert();" class="">Close</a></div></div></div></div>');
    },
    hideAlert: function () {
        $('#lightbox').hide();
        $('#content_box,#lightboxtitle').html('');
    },
    showAlert: function (param) {
        var message = '';
        $(param.messages).each(function (index, value) {
            message += '<li>' + value + '</li>';
        });
        $('#lightbox').fadeIn();
        $('#content_box').html(message);
        $('#lightboxtitle').html(param.title);
        $('#lightboxtitle_box').removeClass().addClass(param.type + 'validation');
        $('#lightboxclose').removeClass().addClass('lightbox_footer_close_btn ligntbox_' + param.type + '_button');
    },
    headerAlert: function (param) {
        $('#headernotify').stop(true, true);
        $('#headernotify').removeClass().addClass('famous-notification famous-' + param.type + '');
        $('#notificationBarLink').text(param.message);
        $('#headernotify').animate({ top: '0px' }, 500);
        if (param.autohide == true)
            setTimeout(function () { Alert.hideheaderAlert(); }, 5000);
    },
    hideheaderAlert: function () {
        $('#headernotify').stop(true, true);
        $('#headernotify').animate({ top: '-40px' }, 500);

    },
    geUsertNotification: function (type) {
        var option = {};
        option.showLoading = false;
        option.HttpVerb = "GET";
        option.async = 'async';
        option.Data = "";
        option.url = "../../UserNotification.txt";
        $.fn.AjaxCommon(option, function (data) {
            var jsonBlog = jQuery.parseJSON(data);
            var message = jsonBlog[type];
            if (message != '') { $('body').append('<div class="bottomNoti" id="bottomNoti"><span class="bottomNoticlose" onclick="Alert.removeUserNotification();">X</span>' + jsonBlog[type] + '</div>'); }
        });
    },
    removeUserNotification: function () { $('#bottomNoti').remove(); }
}

//Common method for ajax service handling
$.fn.AjaxCommon = function (options, Callback) {
    if (options.showLoading == true)
        $.fn.showLoader('Loading Please wait....');

    $.ajax({
        type: options.HttpVerb,
        url: options.url,
        data: options.Data,
        contentType: "application/json; charset=utf-8",
        async: options.async,
        success: function (data, textStatus, xhr) {

            if (options.showLoading == true)
                $.fn.hideLoader();

            if (xhr.status == 200) {
                return Callback(data);
            }
            return req;
        },
        error: function (error) {
            if (options.showLoading == true) {
                $.fn.hideLoader();
                Alert.showAlert({ type: 'error', title: 'Alert !', messages: ["We are sorry. Something wrong happened. Please try after some time."] });
            }
            if (options.onError != null) { options.onError(); }
        }
    });
}

//Method to show/hide Bottom loader
$.fn.hideLoader = function () { $("#divloader").remove(); }
$.fn.showLoader = function (message) {
    if ($("#divloader").length) { $.fn.hideLoader(); }
    var Loader = "<div id='divloader' class='Notiinfo Notimessage'> <p>" + message + "</p> </div>";
    $("body").append(Loader);
}

//Method to get cookie detail
function getCookie(name) { return $.cookie(name); }
function passwordStrength(password) {
    //initial strength
    var strength = 0
    //if the password length is less than 6, return message.
    if (password.length < 6) { return 0; }
    //length is ok, lets continue.
    //if length is 8 characters or more, increase strength value
    if (password.length > 7) strength += 1
    //if password contains both lower and uppercase characters, increase strength value
    if (password.match(/([a-z].*[A-Z])|([A-Z].*[a-z])/)) strength += 1
    //if it has numbers and characters, increase strength value
    if (password.match(/([a-zA-Z])/) && password.match(/([0-9])/)) strength += 1
    //if it has one special character, increase strength value
    if (password.match(/([!,%,&,@,#,$,^,*,?,_,~])/)) strength += 1
    //if it has two special characters, increase strength value
    if (password.match(/(.*[!,%,&,@,#,$,^,*,?,_,~].*[!,%,&,@,#,$,^,*,?,_,~])/)) strength += 1
    //now we have calculated strength value, we can return messages

    //if value is less than 2
    //if (strength < 2) { $('#result').removeClass(); $('#result').addClass('weak'); return 'Weak' }
    //else if (strength == 2) { $('#result').removeClass(); $('#result').addClass('good'); return 'Good'; }
    //else { $('#result').removeClass(); $('#result').addClass('strong'); return 'Strong'; }

    return strength;

}