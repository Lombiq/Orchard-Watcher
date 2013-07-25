(function ($) {
    $.extend(true, {
        lombiq: {
            watcher: {
                _watchUrl: "",
                _unwatchUrl: "",
                _commonPostValues: null,

                init: function (watchUrl, unwatchUrl) {
                    this._watchUrl = watchUrl;
                    this._unwatchUrl = unwatchUrl;
                    this._commonPostValues = { "__RequestVerificationToken": $("input[name='__RequestVerificationToken']").val() };

                    var that = this;
                    $(".lombiq-watcher-watch-link").click(function () {
                        var itemId = $(this).attr("data-item-id");
                        var link = $(this);

                        if (link.hasClass("not-watching")) {
                            that.watch(itemId)
							.done(function (response) {
							    if (!that._validateResponse(itemId, response)) return;
							    that._setWatchCount(itemId, response.WatcherCount);
							    link.removeClass("not-watching");
							    link.addClass("watching");
							});
                        }
                        else {
                            that.unwatch(itemId)
							.done(function (response) {
							    if (!that._validateResponse(itemId, response)) return;
							    that._setWatchCount(itemId, response.WatcherCount);
							    link.removeClass("watching");
							    link.addClass("not-watching");
							});
                        }

                        return false;
                    });
                },

                watch: function (itemId) {
                    return $.post(this._watchUrl + "?itemId=" + itemId, this._commonPostValues);
                },

                unwatch: function (itemId) {
                    return $.post(this._unwatchUrl + "?itemId=" + itemId, this._commonPostValues);
                },

                _setWatchCount: function (itemId, count) {
                    this._getControls(itemId).find(".lombiq-watcher-count").text("(" + count + ")");
                },

                _validateResponse: function (itemId, response) {
                    var $controls = this._getControls(itemId);

                    if (response.WatcherCount === undefined) {
                        $controls.addClass("error");
                        return false;
                    }

                    $controls.removeClass("error");
                    return true;
                },

                _getControls: function (itemId) {
                    return $(".lombiq-watcher-controls-" + itemId);
                }
            }
        }
    });
})(jQuery);