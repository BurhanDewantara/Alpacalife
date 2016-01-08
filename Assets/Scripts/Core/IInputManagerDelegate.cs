using UnityEngine;
using System.Collections;

namespace Artoncode.Core {

	public interface IInputManagerDelegate {
		void touchStateChanged (TouchInput []touches);
	}
}
