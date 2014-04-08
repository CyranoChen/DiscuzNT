/// ******************************************************************************************************************
/// ExtendedPlayer - does nothing to extend baseplayer class yet. Add your custom player handling code here.

Type.registerNamespace('ExtendedPlayer');

ExtendedPlayer.Player = function(domElement) {
    ExtendedPlayer.Player.initializeBase(this, [domElement]);  
}
ExtendedPlayer.Player.prototype =  {
}
ExtendedPlayer.Player.registerClass('ExtendedPlayer.Player', EmePlayer.Player);
