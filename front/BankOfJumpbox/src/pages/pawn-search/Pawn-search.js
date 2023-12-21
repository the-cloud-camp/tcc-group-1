import React from 'react'
import { Input } from '@material-ui/core';
import Button from '@material-ui/core/Button';

const PawnSearch = () => {
    const handleClick = () => {
        console.log('gg')
    }

    return (
        <view>
            <div>PawnSearch</div>
            <Input></Input>
            <Button variant="contained" color="primary" onClick={() => handleClick()}>
                submit
            </Button>
        </view>
    )
}

export default PawnSearch