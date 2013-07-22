(function ($) {
	$.extend(true, {
		lombiq: {
			watcher: {
				_watchUrl: "",
				_unwatchUrl: "",
				_commonPostValues: { "__RequestVerificationToken": $("input[name='__RequestVerificationToken']").val() },

				init: function (watchUrl, unwatchUrl) {
					this._watchUrl = watchUrl;
					this._unwatchUrl = unwatchUrl;

					var that = this;
					$(".lombiq-watcher-watch-link").click(function () {
						var itemId = $(this).attr("data-item-id");
						var link = $(this);

						if (link.hasClass("not-watching")) {
							that.watch(itemId)
							.done(function (response) {
								that._setWatchCount(itemId, response.WatcherCount);
								link.removeClass("not-watching");
								link.addClass("watching");
							});
						}
						else {
							that.unwatch(itemId)
							.done(function (response) {
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
					$(".lombiq-watcher-count-" + itemId).text("(" + count + ")");
				}
			}
		}
	});
})(jQuery);