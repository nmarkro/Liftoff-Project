/*Code from https://therockmanexezone.com/ncgen/ by Prof9 (https://github.com/Prof9)*/

{
	var chipIcons = [];
	window.NCGen = {
		init: function() {
			function fillOptions(className, data) {
				var selects = document.getElementsByClassName(className);
				for (var s = 0; s < selects.length; s++) {
					for (var key in data) {
						var option = document.createElement('option');
						option.value = data[key];
						option.innerHTML = data[key];
						selects[s].appendChild(option);
					}
				}
			}
			
			fillOptions('chip', NCData.chips);
			fillOptions('chip-navi', NCData.naviChips);
			fillOptions('operator', NCData.operators);
			fillOptions('code-type', NCData.codeTypes);
			
			var codeTypeSelects = document.getElementsByClassName('code-type');
			for (var s = 0; s < codeTypeSelects.length; s++) {
				codeTypeSelects[s].value = NCData.codeTypes[0x01];
			}
			
			for (var i = 0; i <= 190; i++) {
				chipIcons[i] = new Image(16, 16);
				chipIcons[i].className = 'chip-icon';
				chipIcons[i].src = '../images/ncgen/icon/' + i + '.png';
			}
			for (var i = 200; i <= 248; i++) {
				chipIcons[i] = new Image(16, 16);
				chipIcons[i].className = 'chip-icon';
				chipIcons[i].src = '../images/ncgen/icon/' + i + '.png';
			}
			
			this.update();
		},
		update: function(i) {
			if (typeof i === 'undefined') {
				i = 0;
			}
			
			function getChipMB(className, data) {
				var chipName = document.getElementsByClassName(className)[i].value;
				var chip = data.indexOf(chipName);
				if (chip <= 0) {
					return 0;
				}
				var mb = NCData.mb[chip];
				return mb;
			}
			function updateChipIcon(className, data) {
				var chipName = document.getElementsByClassName(className)[i].value;
				var chip = data.indexOf(chipName);
				if (chip < 0 || (chip > 190 && chip < 200) || chip > 248) {
					chip = 0;
				}
				var img = document.getElementsByClassName('chip-icon-' + className.replace('chip-', ''))[i];
				img.src = chipIcons[chip].src;
			}
			
			var maxMB = getChipMB('chip-navi', NCData.naviChips);
			
			var curMB = 0;
			curMB += getChipMB('chip-1a', NCData.chips);
			curMB += getChipMB('chip-1b', NCData.chips);
			curMB += getChipMB('chip-2a', NCData.chips);
			curMB += getChipMB('chip-2b', NCData.chips);
			curMB += getChipMB('chip-2c', NCData.chips);
			curMB += getChipMB('chip-3a', NCData.chips);
			curMB += getChipMB('chip-3b', NCData.chips);
			curMB += getChipMB('chip-3c', NCData.chips);
			curMB += getChipMB('chip-3d', NCData.chips);
			
			document.getElementsByClassName('mb-current')[i].innerHTML = curMB;
			document.getElementsByClassName('mb-max')[i].innerHTML = maxMB;
			
			var mbElem = document.getElementsByClassName('mb')[i];
			if (curMB > maxMB) {
				mbElem.style.fontWeight = 'bold';
				mbElem.style.color = 'red';
			} else {
				mbElem.style.fontWeight = '';
				mbElem.style.color = '';
			}
			
			var operator = this.util.getValue(i, 'operator', NCData.operators);
			document.getElementsByClassName('slotin')[i].innerHTML = NCData.slotIn[operator];
			
			var invalid = false;
			if (operator < 0x32 || operator >= 0x38) {
				invalid = true;
			}
			
			document.getElementsByClassName('invalid')[i].style.display = invalid ? 'inline' : 'none';
			
			updateChipIcon('chip-r'   , NCData.chips);
			updateChipIcon('chip-navi', NCData.naviChips);
			updateChipIcon('chip-l'   , NCData.chips);
			updateChipIcon('chip-1a'  , NCData.chips);
			updateChipIcon('chip-1b'  , NCData.chips);
			updateChipIcon('chip-2a'  , NCData.chips);
			updateChipIcon('chip-2b'  , NCData.chips);
			updateChipIcon('chip-2c'  , NCData.chips);
			updateChipIcon('chip-3a'  , NCData.chips);
			updateChipIcon('chip-3b'  , NCData.chips);
			updateChipIcon('chip-3c'  , NCData.chips);
			updateChipIcon('chip-3d'  , NCData.chips);
		},
		
		msgTimers: [],
		msg: function(msg, i) {
			if (typeof i === 'undefined') {
				i = 0;
			}
			
			var elem = document.createElement("DIV");
			elem.innerHTML = msg;
			elem.style.opacity = '1';
			
			while (this.msgTimers.length) {
				clearTimeout(this.msgTimers[0]);
				this.msgTimers.splice(0, 1);
			}
			
			var msgs = document.getElementsByClassName('msg')[i];
			
			this.msgTimers = [
				setTimeout(function() {
					elem.style.opacity = '0';
				}, 5000),
				setTimeout(function() {
					msgs.removeChild(elem);
				}, 6000)
			];
			
			while (msgs.firstChild) {
				msgs.removeChild(msgs.firstChild);
			}
			msgs.appendChild(elem);
			
			console.log(msg);
		},
		
		generateNaviCode: function(i) {
			if (typeof i === 'undefined') {
				i = 0;
			}
			
			var name = document.getElementsByClassName('name')[i].value.toUpperCase();
			if (name.length < 1 || name.length > 4) {
				this.msg('ERROR: Name must be between 1 and 4 characters long.', i);
				return;
			}
			if (this.util.getNameBytes(name).indexOf(undefined) >= 0) {
				this.msg('ERROR: Name contains illegal characters.', i);
				return;
			}
			
			var data = [];
			
			var operator = this.util.getValue(i, 'operator', NCData.operators);
			var naviChip = this.util.getValue(i, 'chip-navi', NCData.naviChips);
			if (NCData.operators[operator] == NCData.naviChips[naviChip]) {
				naviChip = 0;
			}
			
			data[ 0] = operator;
			data[ 1] = naviChip;
			data[ 2] = this.util.getValue(i, 'chip-1a'  , NCData.chips    );
			data[ 3] = this.util.getValue(i, 'chip-1b'  , NCData.chips    );
			data[ 4] = this.util.getValue(i, 'chip-2a'  , NCData.chips    );
			data[ 5] = this.util.getValue(i, 'chip-2b'  , NCData.chips    );
			data[ 6] = this.util.getValue(i, 'chip-2c'  , NCData.chips    );
			data[ 7] = this.util.getValue(i, 'chip-3a'  , NCData.chips    );
			data[ 8] = this.util.getValue(i, 'chip-3b'  , NCData.chips    );
			data[ 9] = this.util.getValue(i, 'chip-3c'  , NCData.chips    );
			data[10] = this.util.getValue(i, 'chip-3d'  , NCData.chips    );
			data[11] = this.util.getValue(i, 'chip-r'   , NCData.chips    );
			data[12] = this.util.getValue(i, 'chip-l'   , NCData.chips    );
			data[13] = this.util.getValue(i, 'code-type', NCData.codeTypes);
			
			var naviCode = this.packNaviCode(name, data);
			document.getElementsByClassName('name')[i].value = name;
			document.getElementsByClassName('code')[i].value = this.util.decorate(naviCode);
			
			//this.msg('Navi code generated!', i);
		},
		
		loadNaviCode: function(i) {
			if (typeof i === 'undefined') {
				i = 0;
			}
			
			var name = document.getElementsByClassName('name')[i].value.toUpperCase();
			if (name.length < 1 || name.length > 4) {
				this.msg('ERROR: Name must be between 1 and 4 characters long.', i);
				return;
			}
			if (this.util.getNameBytes(name).indexOf(undefined) >= 0) {
				this.msg('ERROR: Name contains illegal characters.', i);
				return;
			}
			
			var naviCode = document.getElementsByClassName('code')[i].value;
			naviCode = this.util.normalize(naviCode);
			if (naviCode.length != 24) {
				this.msg('ERROR: Navi code must be 24 characters long (excluding dashes or spaces).', i);
				return;
			}
			
			document.getElementsByClassName('name')[i].value = name;
			document.getElementsByClassName('code')[i].value = this.util.decorate(naviCode);
			
			var data = this.unpackNaviCode(name, naviCode, i);
			if (data.length == 0) {
				this.msg('ERROR: Invalid Navi code.', i);
				return;
			}
			
			this.util.setValue(i, 'operator', NCData.operators, data[0]);
			
			var naviChip = data[1];
			if (naviChip == 0) {
				naviChip = NCData.naviChips.indexOf(NCData.operators[data[0]]);
			}
			this.util.setValue(i, 'chip-navi', NCData.naviChips, naviChip);
			
			this.util.setValue(i, 'chip-1a', NCData.chips, data[2]);
			this.util.setValue(i, 'chip-1b', NCData.chips, data[3]);
			this.util.setValue(i, 'chip-2a', NCData.chips, data[4]);
			this.util.setValue(i, 'chip-2b', NCData.chips, data[5]);
			this.util.setValue(i, 'chip-2c', NCData.chips, data[6]);
			this.util.setValue(i, 'chip-3a', NCData.chips, data[7]);
			this.util.setValue(i, 'chip-3b', NCData.chips, data[8]);
			this.util.setValue(i, 'chip-3c', NCData.chips, data[9]);
			this.util.setValue(i, 'chip-3d', NCData.chips, data[10]);
			this.util.setValue(i, 'chip-r', NCData.chips, data[11]);
			this.util.setValue(i, 'chip-l', NCData.chips, data[12]);
			this.util.setValue(i, 'code-type', NCData.codeTypes, data[13]);
			this.update(i);
			
			//this.msg('Navi code loaded!', i);
		},
		
		unpackNaviCode: function(name, naviCode) {			
			// Get name bytes.
			var nameBytes = this.util.getNameBytes(name);
			// Decode Navi Code.
			var data = this.util.decode(naviCode);
			
			// Decryption step 1.
			data = this.util.crypt(nameBytes, data);
			
			// Verify checksum.
			if (this.util.calcChecksum(data) != data[14]) {
				// Checksum mismatch.
				return [];
			}
			
			// Decryption step 2.
			data = this.util.shift(data, nameBytes[1] + nameBytes[3]);
			data = this.util.crypt(nameBytes, data);
			data = this.util.unshift(data, nameBytes[0] + nameBytes[2]);
			
			return data;
		},
		packNaviCode: function(name, data) {
			// Encode name.
			var nameBytes = this.util.getNameBytes(name);
			
			// Encryption step 1.
			data = this.util.shift(data, nameBytes[0] + nameBytes[2]);
			data = this.util.crypt(nameBytes, data);
			data = this.util.unshift(data, nameBytes[1] + nameBytes[3]);
			
			// Set checksum.
			data[14] = this.util.calcChecksum(data);
			
			// Encryption step 2.
			data = this.util.crypt(nameBytes, data);
			
			// Encode Navi Code.
			var naviCode = this.util.encode(data);
			
			return naviCode;
		},
		
		util: {
			getValue: function(i, className, data) {
				return data.indexOf(document.getElementsByClassName(className)[i].value);
			},
			setValue: function(i, className, data, chip) {
				document.getElementsByClassName(className)[i].value = data[chip];
			},
			getNameBytes: function(name) {
				var nameBytes = [ 0, 0, 0, 0 ];
				for (var i = 0; i < name.length; i++) {
					nameBytes[i] = NCData.charTable[name[i]];
				}
				return nameBytes;
			},
			decode: function(naviCode) {
				var passBytes = [];
				for (var i = 0; i < 24; i++) {
					passBytes[i] = NCData.passChars.indexOf(naviCode[i]);
				}
				
				var data = [];
				for (var i = 14; i >= 0; i--) {
					var b = 0;
					for (var j = 23; j >= 0; j--) {
						var v = passBytes[j] + b * 36
						passBytes[j] = v >> 8;
						b = v & 0xFF;
					}
					data[i] = b;
				}
				return data;
			},
			encode: function(data) {				
				var passBytes = [];
				for (var i = 0; i < 24; i++) {
					var b = 0;
					for (var j = 0; j < 15; j++) {
						var v = data[j] | (b << 8);
						data[j] = ~~(v / 36);
						b = v % 36;
					}
					passBytes[i] = b;
				}
				
				var naviCode = '';
				for (var i = 0; i < 24; i++) {
					naviCode += NCData.passChars[passBytes[i]];
				}
				
				return naviCode;
			},
			crypt: function(nameBytes, data) {
				for (var i = 0; i < 14; i++) {
					data[i] ^= nameBytes[i & 3];
				}
				return data;
			},
			unshift: function(data, bits) {
				if (bits !== 0) {
					var data2 = [];
					for (var i = 0; i < data.length; i++) {
						data2[(i + 2) % data.length] = data[i];
					}
					data = data2;
				}
				return this.shift(data, -bits);
			},
			shift: function(data, bits) {
				var u = (bits >> 3) & 0xF;
				var l = bits & 0x7;
				
				var r = [];
				for (var i = 0; i < 14; i++) {
					r[i]  = (data[(i + u    ) % 14] <<      l ) & 0xFF;
					r[i] |= (data[(i + u + 1) % 14] >> (8 - l)) & 0xFF;
				}
				return r;
			},
			calcChecksum: function(data) {
				var sum = 0;
				for (var i = 0; i < 14; i++) {
					sum += data[i];
				}
				return -sum & 0xFF;
			},
			normalize: function(naviCode) {
				return naviCode.replace(/-/g, '')
				               .replace(/ /g, '')
				               .replace(/♠/g, 'A')
				               .replace(/s/g, 'A')
				               .replace(/♣/g, 'E')
				               .replace(/c/g, 'E')
				               .replace(/♥/g, 'I')
				               .replace(/h/g, 'I')
				               .replace(/★/g, 'O')
				               .replace(/\*/g, 'O')
				               .replace(/♦/g, 'U')
				               .replace(/d/g, 'U');
			},
			decorate: function(naviCode) {
				var result = '';
				for (var i = 0; i < 24; i += 4) {
					if (i > 0) {
						result += '-';
					}
					result += naviCode.substr(i, 4);
				}
				return result;
			},
		},
	}
}