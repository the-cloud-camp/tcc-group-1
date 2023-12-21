import React, { useState } from 'react'
import { Input } from '@material-ui/core';
import Button from '@material-ui/core/Button';
import axios from 'axios';


const PawnSearch = () => {
    const [data,setData] = useState([])
    const handleClick = () => {
        axios.get('https://tcc-01.th1.proen.cloud/bojpawndevback/api/CollateralsTx')
            .then(function (res) {
                console.log('res', res.data.data)
                setData(res.data.data)
            }).then((res) => {
                console.log('hhh',data)
            })
    }

    return (
        <view>
            <div>PawnSearch</div>
            <Input></Input>
            <Button variant="contained" color="primary" onClick={() => handleClick()}>
                submit
            </Button>
            <div>
                data
                {
                    data.map((item) => {
                        return(
                            <ul>
                                <li>{item.store}</li>
                                <li>Status : {item.statusCode}</li>
                            </ul>
                        )
                        {/* item.collateralDetaills.map((item2) =>{
                            return(
                                <ul>
                                    <li>
                                        {item2.collateralItemName}
                                    </li>
                                </ul>
                            )
                        }) */}
                    })
                }
            </div>
        </view>
    )
}

export default PawnSearch