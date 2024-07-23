import { ADD_QUESTION } from "./actions";

const reducer = (state, action) => {
    if(action.type === ADD_QUESTION) {
        return { ...state, error: action.payload };
    }
}

export default reducer;