var dialog = {
    error: function (message, title) {
        layer.open({
            content: message,
            icon: 2,
            title: (title != null && title !== '') ? title : "信息",
            shift: 6
        });
    },
    worning: function (message, title) {
        layer.open({
            content: message,
            icon: 0,
            title: (title != null && title !== '') ? title : "信息",
            shift: 0
        });
    },
    success: function (message, url, title, functionYes) {
        layer.open({
            content: message,
            icon: 1,
            title: (title != null && title !== "") ? title : "信息",
            shift: 5,
            yes: function (index) {
                if (url != null && url !== "") {
                    location.href = url;
                } else {
                    layer.close(index);
                    functionYes();
                }
            }
        });
    },
    question: function (message, title, funtionYes, functionNo) {
        layer.confirm(
			message,
			{
			    icon: 3,
			    title: (title != null && title !== '') ? title : "信息"
			},
			function (index) {
			    funtionYes();
			    layer.close(index);
			},
			function () {
			    functionNo();
			}
		);
    }
}