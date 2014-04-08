

function StartPlayer_0(parentId, playerWidth, playerHeight, mediaUrl, forumPath) {
	playerWidth = parseInt(playerWidth);
	playerHeight = parseInt(playerHeight);
	var xmal = 'silverlight/player/player.xaml';
	if (mediaUrl.indexOf('.mp3') > 0 || mediaUrl.indexOf('.wma') > 0 || mediaUrl.indexOf('.wav') > 0){
		xmal = 'silverlight/player/audio.xaml';
		playerHeight = 33;
	}
    this._hostname = EmePlayer.Player._getUniqueName("xamlHost");
  
    Silverlight.createObjectEx( { source: (typeof(forumpath) == 'undefined' ? '/' : forumpath) + xmal, 
					                  parentElement: $get(parentId ||"divPlayer_0"), 
					                  id:this._hostname, 
					                  properties:{
					                  
					                   width: ((typeof playerWidth == 'undefined')||playerWidth > 800 ? 400 : playerWidth).toString(), 
					                   height:((typeof playerHeight == 'undefined')||playerHeight > 800 ? 300 : playerHeight).toString(),
					                   
					                   
					                    version:'1.0', background:'transparent', isWindowless:'true' }, 
					                  events:{ 
		onLoad:Function.createDelegate(this, function(){
			this._handleLoad();
			var mediainfo = { "mediaUrl": mediaUrl,
                      "placeholderImage": "",
                      "chapters": [] };
			this._player.set_mediainfo(mediainfo);
		}) } } );
}
StartPlayer_0.prototype= {
	_handleLoad: function() {

	//process the UI liuq
	    this.slPlugin = document.getElementById(this._hostname);
		if(this.slPlugin.width<260){
	     this.slPlugin.width=260;
	    }
	    if(this.slPlugin.height<33){
	     this.slPlugin.height=33;
	    }
	
		this.slPlugin_Width=this.slPlugin.width;
		this.slPlugin_Height=this.slPlugin.height;
		//alert("Player Init:"+this.slPlugin_Width+":"+this.slPlugin_Height);    

		
		
		this.slPlugin.content.findName("Main").Width=this.slPlugin_Width;
		this.slPlugin.content.findName("Main").Height=this.slPlugin_Height;
		this.slPlugin.content.findName("VideoBackground").Width=this.slPlugin_Width;
		this.slPlugin.content.findName("VideoBackground").Height=this.slPlugin_Height-this.slPlugin.content.findName("PlayerControls").Height;
		this.slPlugin.content.findName("VideoWindow").Width=this.slPlugin_Width;
		this.slPlugin.content.findName("PlayerControls")["Canvas.Top"]=this.slPlugin_Height-this.slPlugin.content.findName("PlayerControls").Height;
		this.slPlugin.content.findName("VideoWindow").Height=this.slPlugin_Height-this.slPlugin.content.findName("PlayerControls").Height;
		this.slPlugin.content.findName("BarBackground").Width=this.slPlugin_Width;
		this.slPlugin.content.findName("RightControls")["Canvas.Left"]=this.slPlugin_Width-this.slPlugin.content.findName("RightControls").Width;
		this.slPlugin.content.findName("TimeSlider").Width=this.slPlugin_Width-this.slPlugin.content.findName("RightControls").Width-65;
		this.slPlugin.content.findName("DownloadProgressSlider").Width=this.slPlugin_Width-this.slPlugin.content.findName("RightControls").Width-65;

        this._player = $create(   ExtendedPlayer.Player, 
                                  { // properties
                                    autoPlay    : true, 
                                    volume      : 1.0,
                                    muted       : false
                                  
                                  }, 
                                  { // event handlers
                                    mediaEnded: Function.createDelegate(this, this._onMediaEnded)
                                  },
                                  null, $get(this._hostname)  ); 


		//Process Button Mouse Interaction
        
		this.slPlugin.content.findName("PlayPauseButton").addEventListener("MouseEnter", Function.createDelegate(this, function(){this.slPlugin.content.findName('PlayPauseButton_FocusInAnimation').begin()}));
		this.slPlugin.content.findName("PlayPauseButton").addEventListener("MouseLeave", Function.createDelegate(this, function(){this.slPlugin.content.findName('PlayPauseButton_FocusOutAnimation').begin()}));
		
		this.slPlugin.content.findName("TimeThumb").addEventListener("MouseEnter", Function.createDelegate(this, function(){this.slPlugin.content.findName('TimeThumb_FocusInAnimation').begin()}));
		this.slPlugin.content.findName("TimeThumb").addEventListener("MouseLeave", Function.createDelegate(this, function(){this.slPlugin.content.findName('TimeThumb_FocusOutAnimation').begin()}));
		
		this.slPlugin.content.findName("MuteButton").addEventListener("MouseEnter", Function.createDelegate(this, function(){this.slPlugin.content.findName('MuteButton_FocusInAnimation').begin()}));
		this.slPlugin.content.findName("MuteButton").addEventListener("MouseLeave", Function.createDelegate(this, function(){this.slPlugin.content.findName('MuteButton_FocusOutAnimation').begin()}));
	   
		this.slPlugin.content.findName("VolumeThumb").addEventListener("MouseEnter", Function.createDelegate(this, function(){this.slPlugin.content.findName('VolumeThumb_FocusInAnimation').begin()}));
		this.slPlugin.content.findName("VolumeThumb").addEventListener("MouseLeave", Function.createDelegate(this, function(){this.slPlugin.content.findName('VolumeThumb_FocusOutAnimation').begin()}));
		
		this.slPlugin.content.findName("FullScreenButton").addEventListener("MouseEnter", Function.createDelegate(this, function(){this.slPlugin.content.findName('FullScreenButton_FocusInAnimation').begin()}));
		this.slPlugin.content.findName("FullScreenButton").addEventListener("MouseLeave", Function.createDelegate(this, function(){this.slPlugin.content.findName('FullScreenButton_FocusOutAnimation').begin()}));
    
    
	},
	
    _onMediaEnded: function(sender, eventArgs) {

    }

		
}
