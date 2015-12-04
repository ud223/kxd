$(document).ready(function(){
	// 默认展开
	$('.list-group-item-primary.open').each(function(){
		$('.list-group-item-ddl[rel="' + $(this).attr('id') + '"]').show();
	});
	// 绑定展开按钮点击事件
	$('body').on('click', '.list-group-item-primary', function () {
		var tar = $(this).closest('.list-group-item-primary');
		tar.toggleClass('open');
		$('.list-group-item-ddl[rel="' + tar.attr('id') + '"]').toggle();
		// resize 是为了解决nanoscroll的问题
		$(window).resize();
	});
	// 默认展开
	$('.list-group-item.multi.open').each(function(){
		$('.list-group-item-subs[rel="' + $(this).attr('id') + '"]').show();
	});
	// 绑定展开子菜单按钮点击事件
	$('body').on('click', '.list-group-item.multi', function(){
		var tar = $(this).closest('.list-group-item');
		tar.toggleClass('open');
		$('.list-group-item-subs[rel="' + tar.attr('id') + '"]').toggle();
		$(window).resize();
	});
	
	// search 弹出
	$('.nav-itm.top-search').click(function(){
		$(this).toggleClass('selected');
		$('html').toggleClass('html-no-scroll');
		$('.dbmp-box').slideToggle(200);
	});
	$('.dbmp-search-clsbtn').click(function(){
		$('.nav-itm.top-search').click();
	});
	
	// 绑定侧栏隐藏/显示
	$('.side-hide').click(function(){
		$('.side-bar-fix').addClass('go');
		$('.main-body-relative').addClass('full');
		$('.side-hide-back').addClass('back');
		$('.side-bar-fix').removeClass('go-re');
		$('.main-body-relative').removeClass('full-re');
		$('.side-hide-back').removeClass('back-re');
	});
	$('.side-hide-back').click(function(){
		$('.side-bar-fix').addClass('go-re');
		$('.main-body-relative').addClass('full-re');
		$('.side-hide-back').addClass('back-re');
		$('.side-bar-fix').removeClass('go');
		$('.main-body-relative').removeClass('full');
		$('.side-hide-back').removeClass('back');
	});
    //下拉框控件初始化
	$('.dropdown-menu').find('.dropdown-item').click(function () {
	    $(this).parents('.input-group').find('.dropdown-text').val($(this).html());
	    $(this).parents('.input-group').find('.dropdown-value').val($(this).attr('val'));
	})
});


/* FIX TABLE (START) */
$.readjustWidth = function() {
	$('.fix-tb').each(function() {
		var $thistb = $(this);
		var b1 = $thistb.find('.fix-tb-ct-b1');
		var b2 = $thistb.find('.fix-tb-ct-b2');
		var hcels = b1.find('.fix-tb-header-cell');
		var hrow1 = $(b2.find('.fix-tb-body-row').get(0));
		if (!hrow1.length) {
			return;
			//throw "develop error";
		}
		var hbds = hrow1.find('.fix-tb-body-cell');
		if (hcels.length != hbds.length) {
			return;
			throw "develop error";
		}
		for (var i = 0; i < hbds.length; i++) {
			// body cell
			var bc = $(hbds.get(i));
			// header cell
			var hc = $(hcels.get(i));
			var colwidth = bc.width();
			hc.width(colwidth);
		}
	});
};
/* FIX TABLE (END) */

