function initadvertisement()
{

	Silverlight.createObjectEx({
		source: "Pages/Ad.xaml",
		parentElement: document.getElementById("SilverlightControlHost"),
		id: "SilverlightControl",
		properties: {
			width: "568px",
			height: "600px",
			version: "1.1",
			isWindowless:"true",
			background:"transparent",
			enableHtmlAccess: "true"
		},
		events: {}
	});
}
