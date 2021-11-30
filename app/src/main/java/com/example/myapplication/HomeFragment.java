package com.example.myapplication;
import android.content.Context;
import android.os.Bundle;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.fragment.app.Fragment;

import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.ImageButton;
import android.widget.ImageView;
import android.widget.Toast;
import android.widget.ViewFlipper;


public class HomeFragment extends Fragment {

    private View view;

    MainActivity activity;

    @Override
    public void onAttach(Context context) {
        super.onAttach(context);
        //이 함수가 호출될 때는 프래그먼트가 엑티비티위에 올라와있으므로 getActivity()로 MainActivity 참조 가능
        activity = (MainActivity) getActivity();
    }

    @Override
    public void onDetach() {
        super.onDetach();
        //더이상 액티비티 참조 불가능
        activity = null;
    }

    ImageButton prev;
    ImageButton next;
    ViewFlipper flipper;

    ImageView img1;
    ImageView img2;
    ImageView img3;
    Button okBtn;

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater,
                             @Nullable ViewGroup container,
                             @Nullable Bundle savedInstanceState) {
        view = inflater.inflate(R.layout.fragment_home, container, false);

        flipper = (ViewFlipper) view.findViewById(R.id.flipper);
        prev = (ImageButton) view.findViewById(R.id.prev);
        next = (ImageButton) view.findViewById(R.id.next);

        prev.setOnClickListener(this::onClick);
        next.setOnClickListener(this::onClick);

        img1 = (ImageView) view.findViewById(R.id.img1);
        img2 = (ImageView) view.findViewById(R.id.img2);
        img3 = (ImageView) view.findViewById(R.id.img3);

        okBtn = (Button) view.findViewById(R.id.button1);

        //HomeFragment의 ok 버튼에 대한 리스너
        okBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                activity.onFragmentChange(1); //ok 버튼을 클릭하면 map fragment로 전환 됨

                Toast.makeText(getContext(), "맵 선택 화면으로 이동합니다.", Toast.LENGTH_SHORT).show();
            }
        });
        return view;
    }

    //ViewFlipper 클릭 이벤트 핸들러 (좌우 버튼을 누르며 캐릭터 선택)
    public void onClick(View v) {
        if(v == prev) {
            flipper.showPrevious();
        }
        else if(v == next) {
            flipper.showNext();
        }
    }
}
