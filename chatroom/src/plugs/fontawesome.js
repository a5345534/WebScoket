// src/plugins/fontawesome.js

import { library } from '@fortawesome/fontawesome-svg-core';
import { faUserSecret, faCheck, faXmark, faPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';

library.add(faUserSecret, faCheck, faXmark, faPlus);

export { FontAwesomeIcon };
